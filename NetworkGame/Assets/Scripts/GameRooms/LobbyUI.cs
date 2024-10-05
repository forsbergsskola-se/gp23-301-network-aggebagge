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
            roomManager.onJoinedRoom.AddListener(OnJoinedRoom);
            // GuildManager.i.onGuildCreated.AddListener(OnGuildCreated);
            GuildManager.i.onGuildSynced.AddListener(UpdateLobbyGuilds);

        }

        private void OnJoinedRoom()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                startButton.onClick.AddListener(OnClickStart);
                startButton.gameObject.SetActive(true);
                startButton.gameObject.SetActive(true);
            }
            
            codeUITransform.gameObject.SetActive(true);
            codeText.text = roomManager.GetRoomCode();
            
        }

        private void OnClickStart()
        {
            GameManager.i.onStartGame.Invoke();
            lobby.gameObject.SetActive(false);
        }

        // private void OnGuildCreated()
        // {
        //     UpdateLobbyGuilds();
        //     
        //     // roomManager.onUpdatePlayerCount.AddListener(UpdateLobbyGuilds);
        // }

        void UpdateLobbyGuilds()
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if(i >= GuildManager.i.playerGuilds.Count)
                    continue;
                if(lobbyPlayers.Any(lp => lp.id == PhotonNetwork.PlayerList[i].ActorNumber))
                    continue;
                
                if(GuildManager.i.playerGuilds.All(pg => pg.playerID != PhotonNetwork.PlayerList[i].ActorNumber))
                    continue;
                
                var lp = Instantiate(lobbyPlayerPrefab, layout);
                lp.SetupValues(GuildManager.i.playerGuilds[i]);
                
                lobbyPlayers.Add(lp);
            }
        }
        
    }
}