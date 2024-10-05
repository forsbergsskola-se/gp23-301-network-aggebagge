using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace GameSystems.Guild
{
    public class GuildManager : MonoBehaviourPunCallbacks
    {
        [HideInInspector] public UnityEvent onGuildCreated = new();
        [HideInInspector] public UnityEvent onGuildSynced = new();

        public static GuildManager i;

        [Header("Start Values")]
        public int startHp;
        public int startGold;
        public int startGroupSize;
        
        
        [Header("Guild Configurations")]
        [HideInInspector] public List<GuildStats> playerGuilds = new ();
        public string[] guildNames;
        public Color[] guildColors;
        
        private void Awake()
        {
            i = this;
        }
        
        // This method is called when the local player joins the room
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            // If we are not the master client, request the guilds list from the master client
            if (!PhotonNetwork.IsMasterClient)
                photonView.RPC("RequestGuildsFromMaster", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber);
            else
                CreateGuildForPlayer(PhotonNetwork.LocalPlayer.ActorNumber);

        }
        
        private void SyncGuilds()
        {
            // Create an object array to hold the serialized guilds
            Hashtable serializedGuilds = new Hashtable();
            // object[] serializedGuilds = new object[playerGuilds.Count];
    
            for (int i = 0; i < playerGuilds.Count; i++)
            {
                serializedGuilds[i] = playerGuilds[i].ToHashtable(); // Convert each GuildStats to Hashtable
            }

            // Use object[] instead of List<Hashtable>
            photonView.RPC("SyncGuildsRPC", RpcTarget.OthersBuffered, serializedGuilds);
            onGuildSynced.Invoke();
        }
        
        [PunRPC]
        void SyncGuildsRPC(Hashtable serializedGuilds)
        {
            playerGuilds.Clear(); // Clear existing guilds

            foreach (DictionaryEntry guildData in serializedGuilds)
            {
                playerGuilds.Add(GuildStats.FromHashtable((Hashtable)guildData.Value)); // Cast to Hashtable
            }

            Debug.Log("Received guild data: " + serializedGuilds.Count + " guild(s) synced.");
            onGuildSynced.Invoke();
        }
        
        
        // RPC for requesting guilds from the master client
        [PunRPC]
        void RequestGuildsFromMaster(int playerId)
        {
            // Only the master client should respond to this request
            if (PhotonNetwork.IsMasterClient)
            {
                // Call the method to synchronize guilds with the requesting client
                CreateGuildForPlayer(playerId);
            }
        }
        
        private void CreateGuildForPlayer(int playerId)
        {
            int playerIndex = playerId - 1; // Assuming player IDs start from 1
            if (playerIndex >= 0 && playerIndex < guildNames.Length)
            {
                var guildStats = new GuildStats(playerId, guildNames[playerIndex], guildColors[playerIndex]);
                playerGuilds.Add(guildStats);
                Debug.Log($"Created guild for player {playerId}: {guildStats.guildName}");

                // Synchronize the updated guilds list with all players
                SyncGuilds();
            }
            else
            {
                Debug.LogWarning($"Player index {playerIndex} is out of range for guilds.");
            }
        }
        
        public GuildStats GetPlayerStats()
        {
            int id = PhotonNetwork.LocalPlayer.ActorNumber;
            
            var guildStats = playerGuilds.FirstOrDefault(gs => gs.playerID == id);
            // if(guildStats == null)
            //     guildStats = CreateGuild();

            return guildStats;
        }
    }
}