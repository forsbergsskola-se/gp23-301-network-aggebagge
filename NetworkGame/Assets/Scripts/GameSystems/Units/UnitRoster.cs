using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace GameSystems.Units
{
    public class UnitRoster : MonoBehaviour
    {
        public int unitCount;
        public HashSet<UnitSo> unitList = new();
        
        private void Awake()
        {
            var allUnits = Resources.LoadAll<UnitSo>("SO");
            unitList.AddRange(allUnits.Where(unit => unit.cost > 0));
        }
    }
}