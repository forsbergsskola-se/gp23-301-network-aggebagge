using System.Collections;
using GameSystems.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.Battle
{
    public class BattleUnit : UnitUI
    {
        public GameObject popupTextPrefab;
        public GameObject popupIconPrefab;

        public void PopupText(bool isDamage, int value)
        {
            var go = Instantiate(popupTextPrefab, transform);
            go.GetComponent<TextMeshProUGUI>().text = value.ToString();
            go.GetComponent<TextMeshProUGUI>().color = isDamage ? GameManager.i.damageColor : GameManager.i.goldColor;
        }
        
        public void PopupIcon(Sprite icon)
        {
            var go = Instantiate(popupIconPrefab, transform);
            go.GetComponent<Image>().sprite = icon;
        }
    }
}