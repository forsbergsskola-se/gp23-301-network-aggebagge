using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameRooms;
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
        public Sprite[] guildSprite;
        
        private void Awake()
        {
            i = this;
            FindObjectOfType<RoomManager>().onUpdatePlayerCount.AddListener(OnUpdatePlayerCount);
        }

        private void OnUpdatePlayerCount()
        {
            List<GuildStats> itemsToRemove = new List<GuildStats>();

            // Iterate through the playerGuilds list
            foreach (var gs in playerGuilds)
            {
                // If the condition is met, add the item to the removal list
                if (PhotonNetwork.PlayerList.All(player => player.ActorNumber != gs.playerID))
                    itemsToRemove.Add(gs);
            }

            // After the loop, remove the collected items from the original list
            foreach (var item in itemsToRemove)
                playerGuilds.Remove(item);
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            // If we are not the master client, request the guilds list from the master client
            if (!PhotonNetwork.IsMasterClient)
                photonView.RPC("RequestGuildsFromMaster", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber);
            else
                CreateGuildForPlayer(PhotonNetwork.LocalPlayer.ActorNumber);
        }

        // RPC for requesting guilds from the master client
        [PunRPC]
        void RequestGuildsFromMaster(int playerId)
        {
            // Call the method to synchronize guilds with the requesting client
            CreateGuildForPlayer(playerId);
        }

        private void SyncGuilds()
        {
            // Create a Hashtable to hold the serialized guilds
            Hashtable serializedGuilds = new Hashtable();

            for (int i = 0; i < playerGuilds.Count; i++)
            {
                serializedGuilds[i] = playerGuilds[i].ToHashtable(); // Convert each GuildStats to Hashtable
            }

            // Send the serialized guilds as a single Hashtable
            photonView.RPC("SyncGuildsRPC", RpcTarget.All, serializedGuilds);
        }

        [PunRPC]
        void SyncGuildsRPC(Hashtable serializedGuilds)
        {
            playerGuilds.Clear(); // Clear existing guilds

            foreach (DictionaryEntry guildData in serializedGuilds)
                playerGuilds.Add(GuildStats.FromHashtable((Hashtable)guildData.Value)); // Cast to Hashtable

            playerGuilds.Sort((x, y) => x.playerID.CompareTo(y.playerID));
            
            onGuildSynced.Invoke();
        }

        // When creating a guild, add a check for valid indices
        private void CreateGuildForPlayer(int playerId)
        {
            int playerIndex = playerId - 1; // Assuming player IDs start from 1
            if (playerIndex >= 0 && playerIndex < guildNames.Length)
            {
                var guildStats = new GuildStats(playerId, guildNames[playerIndex], guildColors[playerIndex], 0, 0);
                playerGuilds.Add(guildStats);
                Debug.Log($"Created guild for player {playerId}: {guildStats.guildName}");

                // Synchronize the updated guilds list with all players
                SyncGuilds();
            }
        }

        public void UpdateHp(int index, int hp)
        {
            playerGuilds[index].hp = hp;
            SyncGuilds();
        }
        
        public void UpdateGroupSize(int index, int groupSize)
        {
            playerGuilds[index].groupSize = groupSize;
            SyncGuilds();
        }
        
        public GuildStats GetPlayerGuildStats()
        {
            int id = PhotonNetwork.LocalPlayer.ActorNumber;
            var guildStats = playerGuilds.FirstOrDefault(gs => gs.playerID == id);
            return guildStats;
        }

        public Sprite GetGuildSprite(int index)
        {
            return guildSprite[index];
        }
    }
}