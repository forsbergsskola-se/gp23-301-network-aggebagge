using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using GameSystems.Guild;
using GameSystems.Phases;
using GameSystems.Player;
using GameSystems.Units;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GameSystems.Battle
{
    public class BattleRoomManager : MonoBehaviourPunCallbacks
    {
        public static BattleRoomManager i;
        
        [HideInInspector] public UnityEvent<int> onSyncUnitsComplete = new();
        [HideInInspector] public UnityEvent<List<BattleRoom>> onOpponentsPrepared = new();
        private readonly List<BattleRoom> battleRooms = new();
        private readonly List<UnitDataList> unitList = new();
        
        
        private void Awake()
        {
            i = this;
        }

        
        public class BattleRoom
        {
            public GuildStats guild1;
            public GuildStats guild2;

            public readonly List<UnitData> guild1Units = new();
            public readonly List<UnitData> guild2Units = new();

            public void SetBattleRoom(GuildStats g1, GuildStats g2)
            {
                guild1 = g1;
                guild2 = g2;
            }
            public void SetBattleRoom(GuildStats g)
            {
                guild1 = g;
            }

            // public bool ContainsPlayer(int id)
            // {
            //     if (guild1 != null && guild1.playerID == id)
            //         return true;
            //     
            //     if (guild2 != null && guild2.playerID == id)
            //         return true;
            //
            //     return false;
            // }
            //
            // public bool IsPlayerGuild1(int id)
            // {
            //     if (guild1 != null && guild1.playerID == id)
            //         return true;
            //     
            //     return false;
            // }
            //
            // public List<UnitData> GetUnits(int id)
            // {
            //     return IsPlayerGuild1(id)? guild1Units : guild2Units;
            // }
            //
            // public void SetUnits(List<UnitData> units, int id)
            // {
            //     SetUnits(units, IsPlayerGuild1(id)? guild1Units : guild2Units);
            // }

            // private void SetUnits(List<UnitData> units, List<UnitData> list)
            // {
            //     list.Clear();
            //
            //     list.AddRange(units);
            // }
        }
        public class UnitDataList
        {
            public List<UnitData> units = new();

            public UnitDataList()
            {}
        }

        private void Start()
        {
            BattleManager.i.onPlayerEndBattle.AddListener(OnBattleEnd);
            GameManager.i.onStartGame.AddListener(OnStartGame);
        }

        private void OnStartGame()
        {
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
                return;
            
            int rooms = Mathf.CeilToInt(GameManager.i.playersAlive * 0.5f);
            
            for (int r = 0; r < rooms; r++)
                battleRooms.Add(new BattleRoom());

            for (int i = 0; i < GameManager.i.playersAlive; i++)
                unitList.Add(new UnitDataList());
        }

        private void OnBattleEnd()
        {
            for (int room = 0; room < battleRooms.Count; room++)
                battleRooms[0] = new BattleRoom();
        }
        


        public void PrepareBattleOpponents()
        {
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
                return;
            
            int player = 0;
            var guilds = GuildManager.i.playerGuilds;
            
            for (int room = 0; room < battleRooms.Count && player < GameManager.i.playersAlive; room++)
            {
                if (player < GameManager.i.playersAlive - 1)
                {
                    battleRooms[room].SetBattleRoom(guilds[player], guilds[player + 1]);
                }
                else
                {
                    battleRooms[room].SetBattleRoom(guilds[player]);
                }
                player += 2;
            }
            onOpponentsPrepared.Invoke(battleRooms);
        }
        
        
        [PunRPC]
        void SetPlayerUnits(int index, object[] serializedUnits)
        {
            // Create a new list to hold the deserialized UnitData objects
            List<UnitData> units = new List<UnitData>();
    
            // Deserialize each unit
            foreach (object serializedUnit in serializedUnits)
                units.Add(UnitData.FromHashtable((Hashtable)serializedUnit));

            // Store the deserialized list in unitList at the given index
            unitList[index].units = units;

            // Invoke any event to indicate the units have been synced
            onSyncUnitsComplete.Invoke(index);
        }
        
        public void PlayerEndBattle(List<UnitData> units)
        {
            // Convert the UnitData list into an array of Hashtables
            object[] serializedUnits = new object[units.Count];
    
            for (int i = 0; i < units.Count; i++)
            {
                serializedUnits[i] = units[i].ToHashtable();
            }

            // Send the serialized units to all players via an RPC
            photonView.RPC("SetPlayerUnits", RpcTarget.All, GameManager.i.myId, serializedUnits);
        }

        public List<UnitData> GetOpponentUnits(int index)
        {
            return unitList[index].units;
        }
        
        // public void PlayerEndBattle(List<UnitData> units, int battleRoomIndex, bool isGuild1)
        // {
        //     int myIndex = GameManager.i.GetMyPlayerIndex();
        //     
        //     
        //     photonView.RPC("SyncUnits", RpcTarget.MasterClient, units.ToArray(), battleRoomIndex, isGuild1, myIndex);
        // }
        
        // [PunRPC]
        // void SyncUnits(UnitData[] units, int battleRoomIndex, bool isGuild1, int playerIndex)
        // {
        //     battleRooms[battleRoomIndex].SetUnits(units.ToList(), isGuild1);
        //     onSyncUnitsComplete.Invoke(playerIndex);
        // }

        public List<UnitData> GetOpponentUnits(int index, bool isGuild1)
        {
            return isGuild1 ? battleRooms[index].guild1Units : battleRooms[index].guild2Units;
        }
        
    }
}