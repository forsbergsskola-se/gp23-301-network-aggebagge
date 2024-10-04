using System.Collections.Generic;
using System.Linq;
using GameSystems;
using GameSystems.Guild;
using GameSystems.Player;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameRooms
{
    public class LobbyUI : MonoBehaviour
    {
        public RoomManager roomManager;

        public Transform lobby;
        public List<LobbyPlayer> lobbyPlayers = new();
        public LobbyPlayer lobbyPlayerPrefab;
        public Transform layout;

        public TextMeshProUGUI codeText;
        public Button startButton;
        public Transform codeUITransform;
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
                startButton.gameObject.SetActive(true);
                codeUITransform.gameObject.SetActive(true);
                codeText.text = roomManager.GetRoomCode();
            }
        }

        private void OnClickStart()
        {
            GameManager.i.onStartGame.Invoke();
            lobby.gameObject.SetActive(false);
        }

        private void OnUpdatePlayerCount()
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if(lobbyPlayers.Any(lp => lp.id == player.ActorNumber))
                    continue;

                var lp = Instantiate(lobbyPlayerPrefab, layout);
                lp.SetupValues(GuildManager.i.GetPlayerStats());
                
                lobbyPlayers.Add(lp);
            }
        }
        
    }
}