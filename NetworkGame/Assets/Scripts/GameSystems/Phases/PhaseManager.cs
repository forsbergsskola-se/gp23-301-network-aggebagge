using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystems.Phases
{
    public class PhaseManager : MonoBehaviourPunCallbacks
    {
        public UnityEvent OnEndPhase = new();
        
        public Phase phase = Phase.Recruit;
        public BasePhase[] phases;

        public TextMeshProUGUI countdownText;
        public List<bool> playersReady;
        
        public enum Phase
        {
            Lobby,
            Recruit,
            OpponentReveal,
            Prep,
            Battle,
            End
        }

        private void Start()
        {
            GameManager.i.onJoinRoom.AddListener(OnJoinRoom);
        }

        private void OnJoinRoom()
        {
            foreach (var players in GameManager.i.playerIdList)
                playersReady.Add(false);
            
            phase = Phase.Recruit;
            phases[0].OnBeginPhase();
        }

        public void NextPhase()
        {
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
            GameManager.i.GetMyPlayerIndex();
            photonView.RPC("SyncPlayerReady", RpcTarget.Others, GameManager.i.GetMyPlayerIndex());
        }
        
        [PunRPC]
        void SyncPlayersReady(int playerIndex)
        {
            playersReady[playerIndex] = true;

            if (playersReady.All(b => b == true))
                OnEndPhase.Invoke();
        }
        
        void ResetPlayerReady()
        {
            for (int i = 0; i < playersReady.Count; i++)
            {
                playersReady[i] = false;
            }
        }
        
    }
}
