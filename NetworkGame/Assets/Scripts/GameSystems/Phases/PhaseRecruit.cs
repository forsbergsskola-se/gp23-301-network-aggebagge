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
        }

        public override void OnBeginPhase()
        {
            base.OnBeginPhase();
            recruitTransform.gameObject.SetActive(true);
        }

        public override void OnEndPhase()
        {
            base.OnEndPhase();
            recruitTransform.gameObject.SetActive(false);
        }
    }
}