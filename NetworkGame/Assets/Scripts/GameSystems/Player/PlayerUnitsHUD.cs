using System;
using System.Collections.Generic;
using System.Linq;
using GameSystems.Units;
using UnityEngine;

namespace GameSystems.Player
{
    public class PlayerUnitsHUD : MonoBehaviour
    {
        public Transform layout;
        public PlayerUnitUI unitUIPrefab;

        List<PlayerUnitUI> playerUnits = new ();

        private void Start()
        {
            PlayerStats.i.onPlayerSetupComplete.AddListener(Setup);
            PlayerStats.i.onAddUnit.AddListener(OnAddUnit);
        }

        private void Setup()
        {
            foreach (var unitSo in PlayerStats.GetUnits())
                AddUnitToHUD(unitSo);
        }

        private void OnAddUnit()
        {
            var units = PlayerStats.GetUnits();
            AddUnitToHUD(units[^1]);
        }

        private void AddUnitToHUD(UnitSo unitSo)
        {
            PlayerUnitUI playerUnitUI = playerUnits.FirstOrDefault(pUnit => pUnit.unit == unitSo);
            
            if (playerUnitUI == null)
            {
                var unitUI = Instantiate(unitUIPrefab, layout);
                unitUI.SetupUI(unitSo);
                playerUnits.Add(unitUI);
            }
            else
            {
                playerUnitUI.AddUnit();
            }
            
        }
    }
}