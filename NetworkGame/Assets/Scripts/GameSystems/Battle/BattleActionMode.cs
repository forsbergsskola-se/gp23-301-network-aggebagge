using System;
using GameSystems.Units;
using TMPro;
using UnityEngine;

namespace GameSystems.Battle
{
    public class BattleActionMode : MonoBehaviour
    {
        public TextMeshProUGUI actionText;
        private AttributeType type;
        private static BattleActionMode i;

        private BattleUnit actionUnit;
        
        private void Awake()
        {
            i = this;
            
            BattleManager.i.onPlayerEndPrep.AddListener(ExitActionMode);
        }

        private void ExitActionMode()
        {
            i.actionUnit = null;
            i.type = AttributeType.None;

            foreach (var battleUnit in BattleManager.i.playerBattleField.units)
            {
                if(!battleUnit.hasAction)
                    battleUnit.actionButton.interactable = false;
            }
            i.actionText.text = "";
        }

        public static void SetActionType(AttributeType newType, BattleUnit unit)
        {
            i.actionUnit = unit;
            i.type = newType;

            switch (i.type)
            {
                case AttributeType.Buff:
                    i.actionText.text = "Select the unit to receive damage buff"; break;
                case AttributeType.Copy:
                    i.actionText.text = "Select the unit to copy stats from"; break;
                case AttributeType.Kill:
                    i.actionText.text = "Select the unit to remove"; break;
            }
            
            foreach (var battleUnit in BattleManager.i.playerBattleField.units)
                battleUnit.actionButton.interactable = true;
        }
        
        public static void ActionComplete()
        {
            i.actionUnit.uniqueImage.gameObject.SetActive(false);
            i.actionUnit.RemoveAction();
            
            i.ExitActionMode();
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