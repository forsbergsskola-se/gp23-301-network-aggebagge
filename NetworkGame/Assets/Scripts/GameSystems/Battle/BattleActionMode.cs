using System;
using GameSystems.Units;
using UnityEngine;

namespace GameSystems.Battle
{
    public class BattleActionMode : MonoBehaviour
    {
        private AttributeType type;
        private static BattleActionMode i;

        private BattleUnit actionUnit;
        
        private void Awake()
        {
            i = this;
        }

        public static void SetActionType(AttributeType newType, BattleUnit unit)
        {
            i.actionUnit = unit;
            i.type = newType;
            
            foreach (var battleUnit in BattleManager.i.playerBattleField.units)
                battleUnit.actionButton.interactable = true;
        }
        
        public static void ActionComplete()
        {
            i.actionUnit.uniqueImage.gameObject.SetActive(false);
            i.actionUnit.RemoveAction();
            i.actionUnit = null;
            i.type = AttributeType.None;
        }

        public static AttributeType GetActionType()
        {
            return i.type;
        }
        public static BattleUnit GetUnit()
        {
            return i.actionUnit;
        }
    }
}