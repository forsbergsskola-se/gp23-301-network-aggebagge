using System;
using GameSystems.Battle;
using UnityEngine;

namespace GameSystems.Phases
{
    public class PhaseBattle : BasePhase
    {
        public Transform battleTransform;


        private void Start()
        {
            BattleManager.i.onPlayerEndBattle.AddListener(OnEndingPhase);
        }


        public override void OnBeginPhase()
        {
            base.OnBeginPhase();
            canvas.interactable = false;
            BattleManager.i.OnBattlePhaseBegin();
        }
        

        protected override void OnEndPhase()
        {
            base.OnEndPhase();
            battleTransform.gameObject.SetActive(false);
        }
    }
}