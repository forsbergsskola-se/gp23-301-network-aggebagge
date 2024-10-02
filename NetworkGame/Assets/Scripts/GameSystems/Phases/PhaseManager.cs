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
            Recruit,
            Prep,
            Battle
        }

        private void Start()
        {
            phases[0].OnBeginPhase();
        }

        public void NextPhase()
        {
            switch (phase)
            {
                case Phase.Recruit: phase = Phase.Prep;
                    break;
                case Phase.Prep: phase = Phase.Battle;
                    break;
                case Phase.Battle: phase = Phase.Recruit;
                    break;
            }
            
            phases[(int)phase].OnBeginPhase();
        }
    }
}
