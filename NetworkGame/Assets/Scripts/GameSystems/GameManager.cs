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

        [HideInInspector] public UnityEvent<int, int> onPlayerLeaveGame = new ();

        public Color damageColor;
        public Color goldColor;
        
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
        
        
        public void StartGame()
        {
            photonView.RPC("SyncGameStart", RpcTarget.All);
        }
        
        [PunRPC]
        void SyncGameStart()
        {
            onBeginCountdown.Invoke();
            playerIdList.Clear();

            foreach (var player in PhotonNetwork.PlayerList)
            {
                playerIdList.Add(player.ActorNumber);
            }
            FindObjectOfType<RoomManager>().onUpdatePlayerCount.AddListener(OnUpdatePlayerCount);

        }

        private void OnUpdatePlayerCount()
        {
            var removedPlayer = PhotonNetwork.PlayerList.FirstOrDefault(player => !playerIdList.Contains(player.ActorNumber));
            if (removedPlayer != null)
            {
                int id = removedPlayer.ActorNumber;
                int index = GetPlayerIndex(id);
                onPlayerLeaveGame.Invoke(id, index);
                playerIdList.RemoveAt(index);
            }
        }
        
        public bool IsPlayerAlive(int id)
        {
            return playerIdList.Contains(id);
        }
        
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

        public int GetID()
        {
            return PhotonNetwork.LocalPlayer.ActorNumber;
        }
    }
}