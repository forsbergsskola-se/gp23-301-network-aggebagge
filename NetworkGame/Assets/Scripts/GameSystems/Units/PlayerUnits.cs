using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystems.Units
{
    public class PlayerUnits : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<UnitSo> onAddUnit = new();

        public List<UnitSo> guildUnits = new();
        
        public void AddUnit(UnitSo unitSo)
        {
            guildUnits.Add(unitSo);
            onAddUnit.Invoke(unitSo);
        }
    }
}