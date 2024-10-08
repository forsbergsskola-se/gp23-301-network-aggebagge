using System.Collections.Generic;
using System.Linq;
using GameRooms;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystems.Phases
{
    public class PhaseManager : MonoBehaviourPunCallbacks
    {
        public static PhaseManager i;
        [HideInInspector] public UnityEvent onAllPlayersReady = new();
        
        public Phase phase = Phase.Recruit;
        public BasePhase[] phases;

        public TextMeshProUGUI countdownText;
        private List<bool> playersReady = new ();
        public CanvasGroup canvasGroup;
        
        public enum Phase
        {
            Lobby,
            Recruit,
            OpponentReveal,
            Prep,
            Battle,
            End
        }

        private void Awake()
        {
            i = this;
        }

        private void Start()
        {
            GameManager.i.onStartGame.AddListener(OnStartGame);
        }

        private void OnStartGame()
        {
            foreach (var players in GameManager.i.playerIdList)
                playersReady.Add(false);
            
            phase = Phase.Recruit;
            phases[0].OnBeginPhase();
        }
        

        // ReSharper disable Unity.PerformanceAnalysis
        public void NextPhase()
        {
            // StartCoroutine(PrepareNextPhase());
            ResetPlayerReady();

            switch (phase)
            {
                case Phase.Recruit: phase = Phase.OpponentReveal; 
                    phases[1].OnBeginPhase();
                    break;
                case Phase.OpponentReveal: phase = Phase.Prep; 
                    phases[2].OnBeginPhase();
                    break;
                case Phase.Prep: phase = Phase.Battle;
                    phases[3].OnBeginPhase();
                    break;
                case Phase.Battle: phase = Phase.Recruit;
                    phases[0].OnBeginPhase();
                    break;
            }
        }
        
        

        public void PlayerReady()
        {
            PlayerReady(GameManager.i.GetMyPlayerIndex());
        }
        
        public void PlayerReady(int index)
        {
            photonView.RPC("SyncPlayersReady", RpcTarget.All, index);
            if(GameManager.i.GetMyPlayerIndex() == index)
                canvasGroup.interactable = false;
        }
        
        [PunRPC]
        void SyncPlayersReady(int playerIndex)
        {
            if(playerIndex >= 0)
                playersReady[playerIndex] = true;
            
            if (playersReady.All(b => b == true))
                onAllPlayersReady.Invoke();
        }
        
        
        
        void ResetPlayerReady()
        {
            for (int i = 0; i < playersReady.Count; i++)
                playersReady[i] = false;
        }
        
    }
}
