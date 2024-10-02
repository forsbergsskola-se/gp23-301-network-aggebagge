using System.Collections.Generic;
using GameSystems.Units;
using UnityEngine;

namespace GameSystems.Battle
{
    public class BattleFieldUI : MonoBehaviour
    {
        public Transform layout;
        public BattleUnit battleUnitPrefab;
        public GameObject fieldSlotPrefab;

        private Queue<GameObject> fieldSlots = new ();
        [HideInInspector] public List<BattleUnit> battleUnits = new();
        public int slotCount;
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
        }
    }
}