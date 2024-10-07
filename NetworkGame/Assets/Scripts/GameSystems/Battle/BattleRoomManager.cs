using System.Collections;
using System.Collections.Generic;
using GameSystems.Guild;
using GameSystems.Units;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace GameSystems.Battle
{
    public class BattleRoomManager : MonoBehaviourPunCallbacks
    {
        public static BattleRoomManager i;
        
        [HideInInspector] public UnityEvent<int> onSyncUnitsComplete = new();
        [HideInInspector] public UnityEvent onOpponentsPrepared = new();
        // private readonly List<BattleRoom> battleRooms = new();
        private readonly List<OpponentUnits> opponentUnits = new();
        // Initialize the battle manager
        OpponentManager opponentManager;
        

        private void Awake()
        {
            i = this;
            opponentManager = GetComponent<OpponentManager>();
        }
        
        // public class BattleRoom
        // {
        //     public int guild1Index;
        //     public int guild2Index;
        //     
        //
        //     public void SetBattleRoomPlayers(int index1, int index2)
        //     {
        //         guild1Index = index1;
        //         guild2Index = index2;
        //     }
        //     
        //     public void SetBattleRoomPlayers(int index)
        //     {
        //         guild1Index = index;
        //         guild2Index = -1;
        //     }
        //     
        //     // Convert BattleRoom to a Hashtable for Photon
        //     public Hashtable ToHashtable()
        //     {
        //         Hashtable data = new Hashtable();
        //         data["guild1Index"] = guild1Index;
        //         data["guild2Index"] = guild2Index;
        //         return data;
        //     }
        //
        //     // Create a BattleRoom from a Hashtable
        //     public static BattleRoom FromHashtable(Hashtable data)
        //     {
        //         BattleRoom room = new BattleRoom
        //         {
        //             guild1Index = (int)data["guild1Index"],
        //             guild2Index = (int)data["guild2Index"]
        //         };
        //         return room;
        //     }
        // }
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

            
            opponentManager.PrepareOpponentTrackers();
        }

        public void PrepareBattleOpponents()
        {
            opponentManager.SetupBattles();
    
            // Create a Hashtable to hold the serialized opponent trackers
            Hashtable serializedTrackers = new Hashtable();

            for (int i = 0; i < opponentManager.opponentTrackerList.Count; i++)
            {
                serializedTrackers[i] = opponentManager.opponentTrackerList[i].ToHashtable(); // Convert each OpponentTracker to Hashtable
            }
    
            // Send the serialized opponent trackers as a single Hashtable
            photonView.RPC("SyncOpponentsData", RpcTarget.All, serializedTrackers);
        }
        
        [PunRPC]
        void SyncOpponentsData(Hashtable serializedTrackers)
        {
            opponentManager.opponentTrackerList.Clear(); // Clear the list before syncing new data
    
            // Convert each Hashtable back into an OpponentTracker and add it to the list
            foreach (DictionaryEntry trackerData in serializedTrackers)
            {
                OpponentTracker tracker = OpponentTracker.FromHashtable((Hashtable)trackerData.Value);
                opponentManager.opponentTrackerList.Add(tracker);
            }

            onOpponentsPrepared.Invoke();
            Debug.Log("Opponents data synchronized with all players.");
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
            return opponentUnits[GameManager.i.GetPlayerIndex(GetOpponentId())].units;
        }

        private int GetOpponentId()
        {
            return opponentManager.GetMyOpponentID();
        }

        public GuildStats GetOpponentGuildStats()
        {
            int index = GetOpponentId();
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
        private void OnBattleEnd()
        {
            // for (int room = 0; room < battleRooms.Count; room++)
            //     battleRooms[0] = new BattleRoom();
        }
        
    }
}
