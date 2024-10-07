using System;
using System.Collections.Generic;
using System.Linq;
using GameSystems.Player;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameSystems.Units
{
    public class UnitManager : MonoBehaviour
    {
        private static UnitManager i;
        public int unitCount;
        private List<UnitSo> unitList = new();
        private HashSet<UnitData> shopUnits = new();

        public List<UnitSo> startUnits;
        private List<UnitData> startUnitDataList = new();

        private void Awake()
        {
            i = this;
            
            var allUnits = Resources.LoadAll<UnitSo>("SO");
            unitList.AddRange(allUnits);

            while (unitList.Count > 12)
            {
                int index = Random.Range(0, unitList.Count);

                if (!unitList[index].mustBeInShop)
                    unitList.RemoveAt(index);
            }
            
            Debug.Log(unitList.Count());
            
            foreach (var unitSo in unitList)
            {
                if (unitSo.cost > 0)
                    shopUnits.Add(new UnitData(unitSo));
            }

            foreach (var startUnit in startUnits)
                startUnitDataList.Add(new UnitData(startUnit));
        }
        

        public static UnitSo GetUnitSo(int id)
        {
            return i.unitList.FirstOrDefault(unit => unit.id == id);
        }

        public static HashSet<UnitData> GetShopUnits()
        {
            return i.shopUnits;
        }

        public static List<UnitData> GetStartUnits()
        {
            return i.startUnitDataList;
        }
        
    }
}