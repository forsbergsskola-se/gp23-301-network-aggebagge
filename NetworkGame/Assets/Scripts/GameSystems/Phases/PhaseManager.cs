using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameSystems.Guild;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

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
            Debug.Log("NEXT PHASE: " + phase);

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
        }
        
        [PunRPC]
        void SyncPlayersReady(int playerIndex)
        {
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
