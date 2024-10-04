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

        private bool isEndingPhase = false;
        [HideInInspector] public CanvasGroup canvas;

        public virtual void Awake()
        {
            pm = GetComponent<PhaseManager>();
            pm.OnAllPlayersReady.AddListener(OnEndingPhase);
            canvas = FindObjectOfType<CanvasGroup>();
        }

        public virtual void OnBeginPhase()
        {            
            Debug.Log("Begin" + phase);
            canvas.interactable = true;
            isEndingPhase = false;
            countdown = phaseDuration;
            pm.countdownText.text = Mathf.CeilToInt(countdown).ToString();
            pm.countdownText.gameObject.SetActive(phaseDuration > 0);
        }

        protected virtual void OnEndingPhase()
        {
            if (phase != pm.phase)
                return;
            Debug.Log("End" + phase);

            isEndingPhase = true;
            canvas.interactable = false;
            pm.countdownText.gameObject.SetActive(false);
            StartCoroutine(EndPhaseDelay());
        }

        protected virtual void OnEndPhase()
        {
            if (phase != pm.phase)
                return;

            pm.NextPhase();
        }

        private IEnumerator EndPhaseDelay()
        {
            yield return new WaitForSeconds(2);

            if (phase == pm.phase)
                OnEndPhase();
        }


        private void Update()
        {
            if (phase != pm.phase || isEndingPhase)
                return;

            if (phaseDuration > 0)
            {
                countdown -= Time.deltaTime;
                pm.countdownText.text = Mathf.CeilToInt(countdown).ToString();
                
                if(countdown <= 0)
                    OnEndingPhase();
            }
        }
    }
}