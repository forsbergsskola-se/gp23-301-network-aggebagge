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
        private HashSet<UnitSo> unitList = new();
        private HashSet<UnitData> shopUnits = new();

        public List<UnitSo> startUnits;
        private List<UnitData> startUnitDataList = new();

        private void Awake()
        {
            i = this;
            
            var allUnits = Resources.LoadAll<UnitSo>("SO");
            unitList.AddRange(allUnits);
            List<UnitSo> shopUnitList = new();
            shopUnitList.AddRange(unitList.Where(unit => unit.cost > 0));

            while (shopUnitList.Count > 11)
            {
                int index = Random.Range(0, shopUnitList.Count);
                if (!shopUnitList[index].mustBeInShop)
                    shopUnitList.RemoveAt(index);
            }
            
            foreach (var unitSo in shopUnitList)
                shopUnits.Add(new UnitData(unitSo));


            foreach (var startUnit in startUnits)
            {
                Debug.Log(startUnit.unitName);
                startUnitDataList.Add(new UnitData(startUnit));
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

        public static List<UnitData> GetStartUnits()
        {
            return i.startUnitDataList;
        }
        
    }
}