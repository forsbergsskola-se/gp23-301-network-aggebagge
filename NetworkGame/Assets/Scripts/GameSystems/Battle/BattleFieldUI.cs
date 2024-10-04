using System.Collections.Generic;
using GameSystems.Units;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GameSystems.Battle
{
    public class BattleFieldUI : MonoBehaviour
    {
        [HideInInspector] public UnityEvent onAddCurse = new();
        [HideInInspector] public UnityEvent onAddAntiCurse = new();
        [HideInInspector] public UnityEvent<int> onAddDamage = new();

        public Transform layout;
        public BattleUnit battleUnitPrefab;
        public GameObject fieldSlotPrefab;

        public List<BattleUnit> units = new();
        
        private Queue<GameObject> fieldSlots = new ();
        
        public void SetupSlots(int slots)
        {
            units.Clear();
            foreach (Transform child in layout)
                Destroy(child.gameObject);
            
            for (int i = 0; i < slots; i++)
            {
                var fieldSlot = Instantiate(fieldSlotPrefab, layout);
                fieldSlots.Enqueue(fieldSlot);
            }
        }

        public BattleUnit AddUnit(UnitData unitData)
        {
            var battleUnit = Instantiate(battleUnitPrefab, fieldSlots.Dequeue().transform);
            battleUnit.SetupUI(unitData);
            units.Add(battleUnit);

            return battleUnit;
        }
    }
}