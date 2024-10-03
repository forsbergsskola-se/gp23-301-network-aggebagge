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

        private Queue<GameObject> fieldSlots = new ();
        
        public void SetupSlots(int slots)
        {
            for (int i = 0; i < slots; i++)
            {
                var fieldSlot = Instantiate(fieldSlotPrefab, layout);
                fieldSlots.Enqueue(fieldSlot);
            }
        }

        public BattleUnit AddUnit(UnitSo unitSo)
        {
            var battleUnit = Instantiate(battleUnitPrefab, fieldSlots.Dequeue().transform);
            battleUnit.SetupUI(unitSo);

            return battleUnit;
            // BattleManager.i.playerBattleStats.AddBattleUnit(unitSo);
            //
            // if(battleUnit.unit.damage > 0)
            //     onAddDamage.Invoke(battleUnit.unit.damage);
            //
            //
            // if (battleUnit.unit.attribute != null)
            // {
            //     if (battleUnit.unit.attribute.type == UnitAttributeSo.AttributeType.Curse)
            //         onAddCurse.Invoke();
            //     else if (battleUnit.unit.attribute.type == UnitAttributeSo.AttributeType.AntiCurse)
            //         onAddAntiCurse.Invoke();
            // }
        }
    }
}