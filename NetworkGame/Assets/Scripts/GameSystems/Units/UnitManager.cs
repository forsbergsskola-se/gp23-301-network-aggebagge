using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace GameSystems.Units
{
    public class UnitManager : MonoBehaviour
    {
        private static UnitManager i;
        public int unitCount;
        private HashSet<UnitSo> unitList = new();
        private HashSet<UnitData> shopUnits = new();
        
        private void Awake()
        {
            i = this;
            
            var allUnits = Resources.LoadAll<UnitSo>("SO");
            unitList.AddRange(allUnits);

            foreach (var unitSo in allUnits)
            {
                if (unitSo.cost > 0)
                {
                    shopUnits.Add(new UnitData(unitSo));
                }
            }
        }

        public static UnitSo GetUnitSo(int id)
        {
            return i.unitList.FirstOrDefault(unit => unit.id == id);
        }

        public static HashSet<UnitData> GetShopUnits()
        {
            return i.shopUnits;
        }
        
    }
}