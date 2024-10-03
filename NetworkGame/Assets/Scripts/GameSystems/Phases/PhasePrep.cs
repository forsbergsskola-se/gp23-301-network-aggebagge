using GameSystems.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.Phases
{
    public class PhasePrep : BasePhase
    {
        public Transform battleTransform;

        public override void OnBeginPhase()
        {
            base.OnBeginPhase();

            battleTransform.gameObject.SetActive(true);
            BattleManager.i.SetupBattle();
        }
        
    }
}