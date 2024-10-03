using GameSystems.Units;
using Unity.Mathematics;
using UnityEngine;

namespace GameSystems.Battle
{
    public class BattleUnit : UnitUI
    {
        public BonusPopup popupPrefab;

        public void PopupText(bool isDamage, int value)
        {
            var bonusPopup = Instantiate(popupPrefab, transform.position + new Vector3(0, 50, 0), quaternion.identity, transform);
            bonusPopup.SetText(isDamage, value);
        }
        
        public void PopupIcon(Sprite icon)
        {
            var bonusPopup = Instantiate(popupPrefab, transform.position + new Vector3(0, 50, 0), quaternion.identity, transform);
            bonusPopup.SetIcon(icon);
        }
    }
}