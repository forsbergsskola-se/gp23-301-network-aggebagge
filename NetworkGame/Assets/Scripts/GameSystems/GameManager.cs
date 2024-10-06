using System;
using System.Collections.Generic;
using System.Linq;
using GameRooms;
using GameSystems.Guild;
using GameSystems.Units;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GameSystems
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [HideInInspector] public UnityEvent onBeginCountdown = new ();
        [HideInInspector] public UnityEvent onStartGame = new ();
        [HideInInspector] public UnityEvent<GuildStats> onUpdatePlayerHp = new ();

        public int playersAlive;
        public Color damageColor;
        public Color goldColor;


        [HideInInspector] public int myId;
        
        // Singleton instance for easy access
        public static GameManager i;
        public List<int> playerIdList = new ();
        
        
        private void Awake()
        {
            if (i == null)
            {
                i = this;
                // DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }

            PhotonCustomTypes.Register();
            // PhotonNetwork.ConnectUsingSettings();
        }




        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            myId = PhotonNetwork.LocalPlayer.ActorNumber;
        }
        
        public void StartGame()
        {
            photonView.RPC("SyncGameStart", RpcTarget.All);
        }
        
        [PunRPC]
        void SyncGameStart()
        {
            onBeginCountdown.Invoke();
            
            playersAlive = PhotonNetwork.PlayerList.Length;

            playerIdList.Clear();

            foreach (var player in PhotonNetwork.PlayerList)
            {
                playerIdList.Add(player.ActorNumber);
            }
        }

        

        // public void PlayerTakeDamage(int damage)
        // {
        //     int index = GetMyPlayerIndex();
        //     playerGuilds[index].hp -= damage;
        //     playerGuilds[index].hp = Mathf.Clamp(playerGuilds[index].hp, 0, playerGuilds[index].maxHp);
        //
        //     if (playerGuilds[index].hp <= 0)
        //     {
        //         //REMOVE PLAYER
        //     }
        //     photonView.RPC("SyncPlayerStats", RpcTarget.All, index, playerGuilds[index].hp, playerGuilds[index].gold);
        //
        //     onUpdatePlayerHp.Invoke(playerGuilds[index]);
        // }
        //
        // public void AddToPlayerGold(int playerId, int goldToAdd)
        // {
        //     int playerIndex = GetPlayerIndex(playerId);
        //     playerGuilds[playerIndex].gold += goldToAdd;
        //     playerGuilds[playerIndex].gold = Mathf.Clamp(playerGuilds[playerIndex].gold, 0, 99);
        //     
        //     photonView.RPC("SyncPlayerStats", RpcTarget.All, playerIndex, playerGuilds[playerIndex].hp, playerGuilds[playerIndex].gold);
        // }
        //
        //
        // [PunRPC]
        // void SyncPlayerStats(int playerIndex, int hp, int gold)
        // {
        //     playerGuilds[playerIndex].hp = hp;
        //     playerGuilds[playerIndex].gold = gold;
        // }
        
        // Method to get stats for a specific player
        // public GuildStats GetPlayerStats(int playerId)
        // {
        //     return playerGuilds[GetPlayerIndex(playerId)];
        // }

        public int GetPlayerIndex(int id)
        {
            for(int i = 0; i < playerIdList.Count; i++)
            {
                if (playerIdList[i] == id)
                    return i;
            }
            
            return -1;
        }

        public int GetMyPlayerIndex()
        {
            return GetPlayerIndex(PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }
}