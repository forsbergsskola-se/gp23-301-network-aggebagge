using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GameSystems.Phases
{
    public class PhaseManager : MonoBehaviourPunCallbacks
    {
        [FormerlySerializedAs("OnEndPhase")] public UnityEvent OnAllPlayersReady = new();
        
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

        // private IEnumerator PrepareNextPhase()
        // {
        //     yield return new WaitForSeconds(1);
        //     ResetPlayerReady();
        //     
        //     switch (phase)
        //     {
        //         case Phase.Recruit: phase = Phase.OpponentReveal; 
        //             phases[1].OnBeginPhase();
        //             break;
        //         case Phase.OpponentReveal: phase = Phase.Prep; 
        //             phases[2].OnBeginPhase();
        //             break;
        //         case Phase.Prep: phase = Phase.Battle;
        //             phases[3].OnBeginPhase();
        //             break;
        //         case Phase.Battle: phase = Phase.Recruit;
        //             phases[0].OnBeginPhase();
        //             break;
        //     }
        // }
        

        public void PlayerReady()
        {
            GameManager.i.GetMyPlayerIndex();
            photonView.RPC("SyncPlayersReady", RpcTarget.All, GameManager.i.GetMyPlayerIndex());
        }
        
        [PunRPC]
        void SyncPlayersReady(int playerIndex)
        {
            playersReady[playerIndex] = true;
            
            if (playersReady.All(b => b == true))
                OnAllPlayersReady.Invoke();
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
