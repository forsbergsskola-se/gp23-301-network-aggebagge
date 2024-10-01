using System.Collections;
using UnityEngine;

namespace GameSystems.Phases
{
    public class BasePhase : MonoBehaviour
    {
        private PhaseManager pm;

        public float phaseDuration;

        public virtual void OnBeginPhase()
        {
            if (phaseDuration > 0)
                StartCoroutine(PhaseTimer());

        }
        
        public virtual void OnEndPhase()
        {
            pm.NextPhase();
        }

        
        IEnumerator PhaseTimer()
        {
            yield return new WaitForSeconds(phaseDuration);
            OnEndPhase();
        }
        
        
        public void SetPhaseManager(PhaseManager phaseManager)
        {
            pm = phaseManager;
        }
    }
}