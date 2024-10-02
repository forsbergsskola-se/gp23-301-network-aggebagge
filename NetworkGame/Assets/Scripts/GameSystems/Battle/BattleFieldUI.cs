using System.Collections.Generic;
using GameSystems.Units;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GameSystems.Battle
{
    public class BattleFieldUI : MonoBehaviour
    {
        public UnityEvent<int> onUpdateCurseCount = new();
        
        public Transform layout;
        public BattleUnit battleUnitPrefab;
        public GameObject fieldSlotPrefab;

        private Queue<GameObject> fieldSlots = new ();
        [HideInInspector] public List<BattleUnit> battleUnits = new();
        [HideInInspector]public int slotCount;
        private int curseCount;
        
        public void SetupSlots(int slots)
        {
            slotCount = slots;
            
            for (int i = 0; i < slots; i++)
            {
                var fieldSlot = Instantiate(fieldSlotPrefab, layout);
                fieldSlots.Enqueue(fieldSlot);
            }
        }

        public void AddUnit(UnitSo unitSo)
        {
            var battleUnit = Instantiate(battleUnitPrefab, fieldSlots.Dequeue().transform);
            battleUnit.SetupUI(unitSo);
            battleUnits.Add(battleUnit);

            if (battleUnit.unit.attribute != null)
            {
                if (battleUnit.unit.attribute.type == UnitAttributeSo.AttributeType.Curse)
                {
                    curseCount++;
                    onUpdateCurseCount.Invoke(curseCount);
                }
                else if (battleUnit.unit.attribute.type == UnitAttributeSo.AttributeType.AntiCurse)
                {
                    curseCount--;
                    onUpdateCurseCount.Invoke(curseCount);
                }
                
            }
        }
    }
}