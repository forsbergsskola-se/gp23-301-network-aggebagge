using System.Collections.Generic;
using System.Linq;
using GameSystems;
using GameSystems.Guild;
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
        public LobbyPlayer lobbyPlayerPrefab;
        public Transform layout;
        public LobbyPlayer myLobbyPlayer;

        public TextMeshProUGUI codeText;
        public Button startButton;
        public Transform codeUITransform;

        [Header("Countdown")] 
        public Transform countdownTransform;
        public TextMeshProUGUI countdownText;
        public int countdownTime;
        private float countdownTimer;
        
        private void Start()
        {
            roomManager.onJoinedRoom.AddListener(OnJoinedRoom);
            // GuildManager.i.onGuildCreated.AddListener(OnGuildCreated);
            GuildManager.i.onGuildSynced.AddListener(UpdateLobbyGuilds);
            GameManager.i.onStartGame.AddListener(OnStartGame);
            GameManager.i.onBeginCountdown.AddListener(OnBeginCountdown);
        }
        
        private void Update()
        {
            if(countdownTimer <= 0)
                return;

            countdownTimer -= Time.deltaTime;
            countdownText.text = Mathf.CeilToInt(countdownTimer).ToString();

            if (countdownTimer <= 0)
            {
                // SoundtrackManager.StopMusic();
                countdownText.gameObject.SetActive(false);
                GameManager.i.onStartGame.Invoke();
            }
        }

        private void OnBeginCountdown()
        {
            codeUITransform.gameObject.SetActive(false);
            countdownTimer = countdownTime;
            countdownTransform.gameObject.SetActive(true);
        }

        private void OnStartGame()
        {
            lobby.gameObject.SetActive(false);
            countdownTransform.gameObject.SetActive(false);
        }

        private void OnJoinedRoom()
        {
            startButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);

            if (PhotonNetwork.IsMasterClient)
            {
                startButton.onClick.AddListener(OnClickStart);
                startButton.gameObject.SetActive(true);
            }
            
            codeUITransform.gameObject.SetActive(true);
            codeText.text = roomManager.GetRoomCode();
            
            SoundtrackManager.PlayMusic(Soundtrack.Lobby);
            
        }

        private void OnClickStart()
        {
            GameManager.i.StartGame();
            startButton.gameObject.SetActive(false);
        }

        // private void OnGuildCreated()
        // {
        //     UpdateLobbyGuilds();
        //     
        //     // roomManager.onUpdatePlayerCount.AddListener(UpdateLobbyGuilds);
        // }

        void UpdateLobbyGuilds()
        {
            foreach (Transform child in layout)
                Destroy(child.gameObject);
            
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if(i >= GuildManager.i.playerGuilds.Count)
                    continue;
                
                if(GuildManager.i.playerGuilds.All(pg => pg.playerID != PhotonNetwork.PlayerList[i].ActorNumber))
                    continue;

                if (GuildManager.i.playerGuilds[i].playerID == PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    myLobbyPlayer.SetupValues(GuildManager.i.playerGuilds[i]);
                    myLobbyPlayer.gameObject.SetActive(true);
                    continue;
                }
                
                var lp = Instantiate(lobbyPlayerPrefab, layout);
                lp.SetupValues(GuildManager.i.playerGuilds[i]);
            }

           
        }
        
    }
}