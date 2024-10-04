using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace GameSystems.Guild
{
    public class GuildManager : MonoBehaviourPunCallbacks
    {
        public static GuildManager i;

        [Header("Start Values")]
        public int startHp;
        public int startGold;
        public int startGroupSize;
        
        
        [Header("Guild Configurations")]
        public List<GuildStats> playerGuilds = new ();
        public string[] guildNames;
        public Color[] guildColors;
        public Color damageColor;
        public Color goldColor;

        private int fields = 6;
        
        private void Awake()
        {
            i = this;
        }

        // public override void OnJoinedRoom()
        // {
        //     base.OnJoinedRoom();
        //     
        //     if (!PhotonNetwork.IsMasterClient)
        //     {
        //         photonView.RPC("RequestGuildsFromMaster", RpcTarget.MasterClient);
        //     }
        // }
        
        
        // This method is called when the local player joins the room
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            // If we are not the master client, request the guilds list from the master client
            if (!PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("RequestGuildsFromMaster", RpcTarget.MasterClient);
            }
            else
            {
                // Initialize guilds for the master client, if necessary
                // InitializeGuilds();
            }
        }

        // Called by new players to request the guild list from the Master Client
        // [PunRPC]
        // void RequestGuildsFromMaster()
        // {
        //     // The Master Client sends the guilds list to the new player
        //     photonView.RPC("SyncGuilds", RpcTarget.OthersBuffered, SerializeGuilds(playerGuilds));
        // }

        // This method will be called to receive the guilds list and update the local player's guilds
        [PunRPC]
        void SyncGuilds(object[] serializedGuilds)
        {
            playerGuilds = DeserializeGuilds(serializedGuilds);
            Debug.Log("Guilds have been synchronized.");
        }

        // Helper function to initialize guilds (only for Master Client)
        // private void InitializeGuilds()
        // {
        //     // Initialize your playerGuilds list here with data
        //     playerGuilds = new List<GuildStats>
        //     {
        //         new GuildStats(, "Guild 1", startHp, startGold);
        //     };
        // }
        
        
        [PunRPC]
        void RequestGuildsFromMaster()
        {
            // The Master Client sends the guilds list to the new player
            photonView.RPC("SyncGuilds", RpcTarget.OthersBuffered, SerializeGuilds(playerGuilds));
        }
        
        // Serialize the GuildStats list into basic types (for sending over Photon)
        private object[] SerializeGuilds(List<GuildStats> guilds)
        {
            object[] serializedGuilds = new object[guilds.Count * fields]; // Assuming each GuildStats has 4 fields

            for (int i = 0; i < guilds.Count; i++)
            {
                serializedGuilds[i * fields] = guilds[i].playerID;
                serializedGuilds[i * fields + 1] = guilds[i].guildName;
                // serializedGuilds[i * fields + 2] = guilds[i].hp;
                // serializedGuilds[i * fields + 3] = guilds[i].gold;
                // serializedGuilds[i * fields + 4] = guilds[i].groupSize;
                
                serializedGuilds[i * fields + 2] = guilds[i].guildColor.r;
                serializedGuilds[i * fields + 3] = guilds[i].guildColor.g;
                serializedGuilds[i * fields + 4] = guilds[i].guildColor.b;
                serializedGuilds[i * fields + 5] = guilds[i].guildColor.a;
            }

            return serializedGuilds;
        }
        
        
        // Deserialize the received guild data into a List<GuildStats>
        private List<GuildStats> DeserializeGuilds(object[] serializedGuilds)
        {
            List<GuildStats> guilds = new List<GuildStats>();

            for (int i = 0; i < serializedGuilds.Length / 4; i++)
            {
                int guildID = (int)serializedGuilds[i * fields];
                string guildName = (string)serializedGuilds[i * fields + 1];
                float r = (float)serializedGuilds[i * fields + 2];
                float g = (float)serializedGuilds[i * fields + 3];
                float b = (float)serializedGuilds[i * fields + 4];
                float a = (float)serializedGuilds[i * fields + 5];

                guilds.Add(new GuildStats(guildID, guildName, new Color(r, g, b, a)));
            }

            return guilds;
        }
        
        
        public GuildStats GetPlayerStats()
        {
            return playerGuilds.FirstOrDefault(gs => gs.playerID == PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }
}