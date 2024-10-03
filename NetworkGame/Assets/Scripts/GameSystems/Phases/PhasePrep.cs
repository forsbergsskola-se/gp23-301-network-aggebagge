using System;
using System.Collections;
using GameSystems.Battle;
using GameSystems.Player;
using TMPro;
using UnityEngine;

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