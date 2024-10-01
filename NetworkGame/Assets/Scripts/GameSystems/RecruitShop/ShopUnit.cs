using GameSystems.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.RecruitShop
{
    public class ShopUnit : MonoBehaviour
    {
        public TextMeshProUGUI costText;
        public UnitUI unitUI;
        private UnitSo unitSo;

        public void SetupUI(UnitSo unit)
        {
            unitSo = unit;
            costText.text = unit.cost.ToString();
            unitUI.SetupUI(unit);
        }



    }
}