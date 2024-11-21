using GameSystems.Units;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameSystems.Battle
{
    public class BattleUnit : UnitUI
    {
        [HideInInspector]public UnityEvent<BattleUnit> onKill = new();

        public BonusPopup popupPrefab;
        public Button actionButton;

        [HideInInspector] public bool hasAction;
        
        public override void SetupUI(UnitData unitData)
        {
            base.SetupUI(unitData);

            if (unitData.attributeType == AttributeType.Buff || unitData.attributeType == AttributeType.Foresight ||
                unitData.attributeType == AttributeType.Kill || unitData.attributeType == AttributeType.Copy)
            {
                actionButton.interactable = true;
                hasAction = true;
            }
            
            actionButton.onClick.AddListener(OnSelect);
        }

        private void OnSelect()
        {
            var actionType = BattleActionMode.GetActionType();

            switch (actionType)
            {
                case AttributeType.Buff: Buff(); break;
                case AttributeType.Kill: Kill(); break;
                case AttributeType.Copy: Copy(BattleActionMode.GetUnit()); break;
                case AttributeType.None: BattleActionMode.SetActionType(data.attributeType, this);
                    break;
            }
        }

        private void UpdateUI()
        {
            if(data.damage > 0)
                damage.text = data.damage.ToString();
            if(data.goldGain > 0)
                gold.text = data.goldGain.ToString();
        }
        
        public void RemoveAction()
        {
            hasAction = false;
            actionButton.interactable = false;
        }

        private void Kill()
        {
            onKill.Invoke(this);
            BattleActionMode.ActionComplete();
            Destroy(gameObject);
        }
        private void Buff()
        {
            data.damage += 1;
            UpdateUI();
            PopupText(true, 1);
            BattleActionMode.ActionComplete();

        }
        private void Copy(BattleUnit actionUnit)
        {
            actionUnit.data.damage = data.damage;
            actionUnit.data.goldGain = data.goldGain;
            actionUnit.UpdateUI();
            BattleActionMode.ActionComplete();
        }
        
        public void PopupText(bool isDamage, int value)
        {
            var bonusPopup = Instantiate(popupPrefab, transform.position + new Vector3(0, 50, 0), quaternion.identity, transform);
            bonusPopup.SetText(isDamage, value);
            GetComponent<AudioSource>().Play();
        }
        
        public void PopupIcon(Sprite icon)
        {
            var bonusPopup = Instantiate(popupPrefab, transform.position + new Vector3(0, 50, 0), quaternion.identity, transform);
            bonusPopup.SetIcon(icon);
            GetComponent<AudioSource>().Play();
        }
    }
}