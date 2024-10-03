using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace GameSystems.Phases
{
    public class BasePhase : MonoBehaviourPunCallbacks
    {
        [HideInInspector] public PhaseManager pm;

        public PhaseManager.Phase phase;
        public float phaseDuration;

        private float countdown;

        public virtual void Awake()
        {
            pm = GetComponent<PhaseManager>();
            pm.OnEndPhase.AddListener(OnEndPhase);
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