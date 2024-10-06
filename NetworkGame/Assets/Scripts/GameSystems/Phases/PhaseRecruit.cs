using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.Phases
{
    public class PhaseRecruit : BasePhase
    {
        public Transform recruitTransform;
        public Button readyButton;

        public override void Awake()
        {
            base.Awake();
            readyButton.onClick.AddListener(pm.PlayerReady);
            readyButton.onClick.AddListener(DisableButton);

        }

        private void DisableButton()
        {
            readyButton.interactable = false;
        }

        public override void OnBeginPhase()
        {
            base.OnBeginPhase();
            readyButton.interactable = true;
            recruitTransform.gameObject.SetActive(true);
        }

        protected override void OnEndPhase()
        {
            base.OnEndPhase();
            recruitTransform.gameObject.SetActive(false);
        }

        protected override void OnEndingPhase()
        {
            base.OnEndingPhase();
        }
    }
}