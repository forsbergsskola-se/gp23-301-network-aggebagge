using System;
using GameSystems.Units;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GameSystems.RecruitShop
{
    public class RecruitShopWindow : MonoBehaviour
    {
        public ShopUnit shopUnitPrefab;
        public Transform unitLayout;

        private void Start()
        {
            var unitList = UnitManager.GetShopUnits();
            
            foreach (var unitData in unitList)
            {
                ShopUnit unitInstance = Instantiate(shopUnitPrefab, unitLayout);
                unitInstance.SetupUI(unitData);
            }
            
            EnableShop(false);
        }

        public void EnableShop(bool enable)
        {
            foreach (Transform child in unitLayout)
                child.gameObject.SetActive(enable);
        }
    }
}