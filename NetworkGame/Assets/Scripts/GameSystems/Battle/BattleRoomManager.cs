using System.Collections;
using System.Collections.Generic;
using GameSystems.Guild;
using GameSystems.Units;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace GameSystems.Battle
{
    public class BattleRoomManager : MonoBehaviourPunCallbacks
    {
        public static BattleRoomManager i;
        
        [HideInInspector] public UnityEvent<int> onSyncUnitsComplete = new();
        [HideInInspector] public UnityEvent onOpponentsPrepared = new();
        private readonly List<BattleRoom> battleRooms = new();
        private readonly List<OpponentUnits> opponentUnits = new();
        // Initialize the battle manager
        OpponentManager opponentManager;
        private void Awake()
        {
            i = this;
        }
        
        public class BattleRoom
        {
            public int guild1Index;
            public int guild2Index;
            

            public void SetBattleRoomPlayers(int index1, int index2)
            {
                guild1Index = index1;
                guild2Index = index2;
            }
            
            public void SetBattleRoomPlayers(int index)
            {
                guild1Index = index;
                guild2Index = -1;
            }
            
            // Convert BattleRoom to a Hashtable for Photon
            public Hashtable ToHashtable()
            {
                Hashtable data = new Hashtable();
                data["guild1Index"] = guild1Index;
                data["guild2Index"] = guild2Index;
                return data;
            }

            // Create a BattleRoom from a Hashtable
            public static BattleRoom FromHashtable(Hashtable data)
            {
                BattleRoom room = new BattleRoom
                {
                    guild1Index = (int)data["guild1Index"],
                    guild2Index = (int)data["guild2Index"]
                };
                return room;
            }
        }
        private class OpponentUnits
        {
            public List<UnitData> units = new();
        }

        private void Start()
        {
            BattleManager.i.onPlayerEndBattle.AddListener(OnBattleEnd);
            GameManager.i.onStartGame.AddListener(OnStartGame);
        }

        private void OnStartGame()
        {
            for (int players = 0; players < PhotonNetwork.PlayerList.Length; players++)
                opponentUnits.Add(new OpponentUnits());

            opponentManager = new OpponentManager(GameManager.i.playerIdList);
            
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
                return;
            
            int rooms = Mathf.CeilToInt(PhotonNetwork.PlayerList.Length * 0.5f);
            
            for (int r = 0; r < rooms; r++)
                battleRooms.Add(new BattleRoom());
        }

        private void OnBattleEnd()
        {
            for (int room = 0; room < battleRooms.Count; room++)
                battleRooms[0] = new BattleRoom();
        }

        public void PrepareBattleOpponents()
        {
            var nextBattle = opponentManager.FindNextBattle();
            if (nextBattle.player1 != -1)
            {
                Debug.Log($"Next Battle: {nextBattle.player1} vs {nextBattle.player2}");

                // Register the battle after it's completed
                opponentManager.RegisterBattle(nextBattle.player1, nextBattle.player2);
            }
            else
            {
                opponentManager.ResetAllBattles();
            }
        }

        // public void PrepareBattleOpponents()
        // {
        //     if (!PhotonNetwork.LocalPlayer.IsMasterClient)
        //         return;
        //     
        //     int player = 0;
        //
        //     for (int room = 0; room < battleRooms.Count && player < GameManager.i.playersAlive; room++)
        //     {
        //         if (player < GameManager.i.playersAlive - 1)
        //             battleRooms[room].SetBattleRoomPlayers(player, player + 1);
        //         else
        //             battleRooms[room].SetBattleRoomPlayers(player);
        //
        //         player += 2;
        //     }
        //
        //     // Now sync the battle rooms with all clients
        //     SyncBattleRoomsWithClients();
        // }

        private void SyncBattleRoomsWithClients()
        {
            // Create a Hashtable to hold serialized battle rooms
            Hashtable serializedBattleRooms = new Hashtable();

            for (int i = 0; i < battleRooms.Count; i++)
            {
                serializedBattleRooms[i.ToString()] = battleRooms[i].ToHashtable(); // Serialize to Hashtable
            }

            // Send the serialized data to all players via RPC
            photonView.RPC("SyncBattleRooms", RpcTarget.All, serializedBattleRooms);
        }

        [PunRPC]
        void SyncBattleRooms(Hashtable serializedBattleRooms)
        {
            // Clear existing battleRooms on the client (if needed)
            battleRooms.Clear();

            // Deserialize each BattleRoom from the Hashtable
            foreach (DictionaryEntry entry in serializedBattleRooms)
            {
                Hashtable roomData = (Hashtable)entry.Value; // Cast to Hashtable
                battleRooms.Add(BattleRoom.FromHashtable(roomData)); // Deserialize to BattleRoom
            }

            // Call an event or method to indicate that the battle rooms are ready
            onOpponentsPrepared.Invoke();
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
            opponentUnits[index].units = units;

            // Invoke any event to indicate the units have been synced
            onSyncUnitsComplete.Invoke(index);
        }
        
        public void PlayerEndBattle(List<UnitData> units)
        {
            // Convert the UnitData list into an array of Hashtables
            object[] serializedUnits = new object[units.Count];
    
            for (int i = 0; i < units.Count; i++)
                serializedUnits[i] = units[i].ToHashtable();

            // Send the serialized units to all players via an RPC
            photonView.RPC("SetPlayerUnits", RpcTarget.All, GameManager.i.GetMyPlayerIndex(), serializedUnits);
        }

        public List<UnitData> GetOpponentUnits()
        {
            return opponentUnits[GetOpponentIndex(GameManager.i.GetMyPlayerIndex())].units;
        }

        private int GetOpponentIndex(int index)
        {
            foreach (var battleRoom in battleRooms)
            {
                Debug.Log(battleRoom.guild1Index + " " + battleRoom.guild2Index + " | " + index);
                
                if(battleRoom.guild1Index == index)
                    return battleRoom.guild2Index;
                if(battleRoom.guild2Index == index)
                    return battleRoom.guild1Index;
            }

            return -1;
        }

        public GuildStats GetOpponentGuildStats()
        {
            int index = GetOpponentIndex(GameManager.i.GetMyPlayerIndex());
            if (index == -1)
                return null;
            return GuildManager.i.playerGuilds[index];
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

        // public List<UnitData> GetOpponentUnits(int index, bool isGuild1)
        // {
        //     return isGuild1 ? battleRooms[index].guild1Units : battleRooms[index].guild2Units;
        // }
        
    }
}