using GameSystems.Battle;
using UnityEngine;

namespace GameSystems.Phases
{
    public class PhasePrep : BasePhase
    {
        public override void OnBeginPhase()
        {
            base.OnBeginPhase();
            
            BattleRoomManager.i.PrepareBattleOpponents();
        }
    }
}