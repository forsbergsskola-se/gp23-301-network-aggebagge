using UnityEngine;

namespace GameSystems.Phases
{
    public class PhaseBattle : BasePhase
    {
        public Transform battleTransform;
        
        public override void OnEndPhase()
        {
            base.OnEndPhase();
            battleTransform.gameObject.SetActive(false);
        }
    }
}