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
        private readonly List<OpponentUnits> opponentUnits = new();
        OpponentManager opponentManager;
        

        private void Awake()
        {
            i = this;
            opponentManager = GetComponent<OpponentManager>();
        }
        
        private class OpponentUnits
        {
            public List<UnitData> units = new();
        }

        private void Start()
        {
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

            foreach (var trackerEntry in opponentManager.opponentTrackers)
                serializedTrackers[trackerEntry.Key] = trackerEntry.Value.ToHashtable(); // Convert each OpponentTracker to Hashtable

            // Send the serialized opponent trackers as a single Hashtable
            photonView.RPC("SyncOpponentsData", RpcTarget.All, serializedTrackers);
        }
        
        [PunRPC]
        void SyncOpponentsData(Hashtable serializedTrackers)
        {
            opponentManager.opponentTrackers.Clear(); // Clear the dictionary before syncing new data

            // Convert each Hashtable back into an OpponentTracker and add it to the dictionary
            foreach (DictionaryEntry trackerData in serializedTrackers)
            {
                int playerId = (int)trackerData.Key; // The key is the playerId
                OpponentTracker tracker = OpponentTracker.FromHashtable((Hashtable)trackerData.Value);
                opponentManager.opponentTrackers[playerId] = tracker; // Add to the dictionary
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
            int index = GameManager.i.GetPlayerIndex(GetOpponentId());
            if (index == -1)
                return null;
            return GuildManager.i.playerGuilds[index];
        }
        
    }
}
