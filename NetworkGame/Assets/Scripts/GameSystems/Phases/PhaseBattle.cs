using UnityEngine;

namespace GameSystems.Phases
{
    public class PhaseBattle : BasePhase
    {
        public Transform battleTransform;

        protected override void OnEndPhase()
        {
            base.OnEndPhase();
            battleTransform.gameObject.SetActive(false);
        }
    }
}