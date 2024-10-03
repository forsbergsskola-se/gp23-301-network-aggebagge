using System;
using System.Collections.Generic;
using System.Linq;
using GameSystems.Guild;
using GameSystems.Units;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GameSystems
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [HideInInspector] public UnityEvent onJoinRoom = new ();
        [HideInInspector] public UnityEvent<GuildStats> onUpdatePlayerHp = new ();

        private int playerCount;
        
        [Header("Start Values")]
        public int startHp;
        public int startGold;
        public int startGroupSize;
        
        [Header("Guild Configurations")]
        public List<GuildStats> playerGuilds = new ();
        public string[] guildNames;
        public Color[] guildColors;
        
        // Singleton instance for easy access
        public static GameManager i;
        public List<int> playerIdList = new ();

        [HideInInspector] public bool hasJoinedRoom;

        private void Awake()
        {
            if (i == null)
            {
                i = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
            
            PhotonNetwork.ConnectUsingSettings();
            

            
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            PhotonNetwork.JoinRandomRoom();
        }
        
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            // Create a room if joining fails
            RoomOptions roomOptions = new RoomOptions { MaxPlayers = 6 };
            PhotonNetwork.CreateRoom(null, roomOptions);
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            
            Debug.Log("JOIN ROOM");
            hasJoinedRoom = true;
            playerCount = PhotonNetwork.PlayerList.Length;

            for (int i = 0; i < playerCount; i++)
            {
                playerGuilds.Add(new GuildStats(startHp, startGold, startGroupSize, guildNames[i], guildColors[i]));
                playerIdList.Add(PhotonNetwork.PlayerList[i].ActorNumber);
            }
            
            onJoinRoom.Invoke();
        }


        public void AddToPlayerHp(int playerId, int hpToAdd)
        {
            int playerIndex = GetPlayerIndex(playerId);

            playerGuilds[playerIndex].hp += hpToAdd;
            playerGuilds[playerIndex].hp = Mathf.Clamp(playerGuilds[playerIndex].hp, 0, playerGuilds[playerIndex].maxHp);

            if (playerGuilds[playerId].hp <= 0)
            {
                //REMOVE PLAYER
            }
            photonView.RPC("SyncPlayerStats", RpcTarget.Others, playerIndex, playerGuilds[playerIndex].hp, playerGuilds[playerIndex].gold);

            onUpdatePlayerHp.Invoke(playerGuilds[playerIndex]);
        }
        
        public void AddToPlayerGold(int playerId, int goldToAdd)
        {
            int playerIndex = GetPlayerIndex(playerId);
            playerGuilds[playerIndex].gold += goldToAdd;
            playerGuilds[playerIndex].gold = Mathf.Clamp(playerGuilds[playerIndex].gold, 0, 99);
            
            photonView.RPC("SyncPlayerStats", RpcTarget.Others, playerIndex, playerGuilds[playerIndex].hp, playerGuilds[playerIndex].gold);
        }
        
        
        [PunRPC]
        void SyncPlayerStats(int playerIndex, int hp, int gold)
        {
            playerGuilds[playerIndex].hp = hp;
            playerGuilds[playerIndex].gold = gold;
        }
        
        // Method to get stats for a specific player
        public GuildStats GetPlayerStats(int playerId)
        {
            return playerGuilds[GetPlayerIndex(playerId)];
        }

        private int GetPlayerIndex(int id)
        {
            return playerIdList.IndexOf(id);
        }

        public int GetMyPlayerIndex()
        {
            return GetPlayerIndex(PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }
}