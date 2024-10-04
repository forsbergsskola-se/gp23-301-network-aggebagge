using System;
using System.Collections.Generic;
using System.Linq;
using GameSystems.Phases;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameRooms
{
    public class LobbyUI : MonoBehaviour
    {
        public RoomManager roomManager;

        public List<LobbyPlayer> lobbyPlayers = new();
        public LobbyPlayer lobbyPlayerPrefab;
        public Transform layout;

        public TextMeshProUGUI codeText;
        public Button startButton;
        
        private void Start()
        {
            OnUpdatePlayerCount();
            roomManager.onUpdatePlayerCount.AddListener(OnUpdatePlayerCount);
            roomManager.onJoinedRoom.AddListener(OnJoinedRoom);
        }

        private void OnJoinedRoom()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                startButton.onClick.AddListener(OnClickStart);
                startButton.gameObject.SetActive(true);

                codeText.text = roomManager.GetRoomCode();
            }
        }

        private void OnClickStart()
        {
            FindObjectOfType<PhaseManager>().NextPhase();
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