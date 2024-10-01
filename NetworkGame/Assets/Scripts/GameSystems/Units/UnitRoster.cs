using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GameSystems.Units
{
    public class UnitRoster : MonoBehaviour
    {
        public int unitCount;
        public HashSet<UnitSo> unitList;
        
        private void Awake()
        {
            var allUnits = Resources.LoadAll<UnitSo>("SO");
            unitList.AddRange(allUnits);
        }
    }
}