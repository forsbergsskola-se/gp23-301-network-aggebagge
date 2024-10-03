using Photon.Pun;
using TMPro;
using UnityEngine;

namespace GameSystems.Phases
{
    public class PhaseManager : MonoBehaviourPunCallbacks
    {
        public Phase phase = Phase.Recruit;
        public BasePhase[] phases;

        public TextMeshProUGUI countdownText;
        
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
            phase = Phase.Recruit;
            phases[0].OnBeginPhase();
        }

        public void NextPhase()
        {
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
        
        
    }
}
