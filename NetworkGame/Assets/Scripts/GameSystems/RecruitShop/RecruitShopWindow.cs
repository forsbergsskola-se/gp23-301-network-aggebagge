using System;
using GameSystems.Units;
using UnityEngine;

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
        }
    }
}