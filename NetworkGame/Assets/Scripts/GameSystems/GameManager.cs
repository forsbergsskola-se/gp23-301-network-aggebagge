using System;
using System.Collections.Generic;
using System.Linq;
using GameSystems.Guild;
using GameSystems.Units;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystems
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [HideInInspector] public UnityEvent onJoinRoom = new ();
        [HideInInspector] public UnityEvent<int, int> onUpdatePlayerHp = new ();

        public int playerCount;
        public int playerHp;
        public int startGold;
        public List<GuildStats> playerGuilds = new ();
        public string[] guildNames;
        public Color[] guildColors;
        
        // Singleton instance for easy access
        public static GameManager i;
        public List<int> playerIdList = new ();
        
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
            Debug.Log("COONNECTED");
            PhotonNetwork.JoinRandomRoom();
        }
        
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to join random room: " + message);
            // Create a room if joining fails
            RoomOptions roomOptions = new RoomOptions { MaxPlayers = 6 };
            PhotonNetwork.CreateRoom(null, roomOptions);
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            Debug.Log("JOIN ROOM");

            playerCount = PhotonNetwork.PlayerList.Length;
            Debug.Log(playerCount);

            for (int i = 0; i < playerCount; i++)
            {
                playerGuilds.Add(new GuildStats(playerHp, startGold, guildNames[i], guildColors[i]));
                playerIdList.Add(PhotonNetwork.PlayerList[i].ActorNumber);
                Debug.Log(PhotonNetwork.PlayerList[i].ActorNumber);
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

            onUpdatePlayerHp.Invoke(playerIndex, playerGuilds[playerIndex].hp);
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
            Debug.Log(playerId);
            return playerGuilds[GetPlayerIndex(playerId)];
        }

        private int GetPlayerIndex(int id)
        {
            return playerIdList.IndexOf(id);
        }
    }
}