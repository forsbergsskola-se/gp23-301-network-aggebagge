using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace GameRooms
{
    public class LobbyUI : MonoBehaviour
    {
        public RoomManager roomManager;

        public List<LobbyPlayer> lobbyPlayers = new();
        public LobbyPlayer lobbyPlayerPrefab;

        private Transform layout;

        public Color[] color;
        public Sprite[] crests;
        
        private void Start()
        {
            OnUpdatePlayerCount();
            roomManager.onUpdatePlayerCount.AddListener(OnUpdatePlayerCount);
        }

        private void OnUpdatePlayerCount()
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if(lobbyPlayers.Any(lp => lp.id == player.ActorNumber))
                    continue;

                Instantiate(lobbyPlayerPrefab, layout);

            }
        }
        
    }
}