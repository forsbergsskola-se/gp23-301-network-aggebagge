using System;
using System.Collections;
using UnityEngine;

namespace GameSystems.Phases
{
    public class BasePhase : MonoBehaviour
    {
        private PhaseManager pm;

        public PhaseManager.Phase phase;
        public float phaseDuration;

        private float countdown;

        private void Awake()
        {
            pm = GetComponent<PhaseManager>();
        }

        public virtual void OnBeginPhase()
        {
            countdown = phaseDuration;
            pm.countdownText.text = Mathf.CeilToInt(countdown).ToString();
            pm.countdownText.gameObject.SetActive(phaseDuration > 0);
            
        }
        
        public virtual void OnEndPhase()
        {
            pm.NextPhase();
        }


        private void Update()
        {
            if (phase != pm.phase)
                return;

            if (phaseDuration > 0)
            {
                countdown -= Time.deltaTime;
                pm.countdownText.text = Mathf.CeilToInt(countdown).ToString();
                
                if(countdown <= 0)
                    OnEndPhase();
            }
        }
    }
}