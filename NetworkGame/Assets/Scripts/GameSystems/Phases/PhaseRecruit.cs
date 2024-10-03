using UnityEngine;

namespace GameSystems.Phases
{
    public class PhaseRecruit : BasePhase
    {
        public Transform recruitTransform;

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