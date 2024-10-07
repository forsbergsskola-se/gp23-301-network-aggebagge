using System;
using System.Collections.Generic;
using System.Linq;
using GameSystems.Battle;
using GameSystems.Units;
using UnityEngine;

namespace GameSystems.Player
{
    public class PlayerUnitsHUD : MonoBehaviour
    {
        public Transform layout;
        public PlayerUnitUI unitUIPrefab;

        List<PlayerUnitUI> playerUnits = new ();
        Queue<UnitData> deployedUnits = new ();

        private void Start()
        {
            PlayerStats.i.onPlayerSetupComplete.AddListener(Setup);
            PlayerStats.i.onAddUnit.AddListener(OnAddUnit);
            BattleManager.i.playerBattleStats.onDeployUnit.AddListener(RemoveUnitFromHUD);
            BattleManager.i.onPlayerEndBattle.AddListener(ReturnDeployedUnits);
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

        public void ReturnDeployedUnits()
        {
            Debug.Log(deployedUnits.Count);
            for (int i = 0; i < deployedUnits.Count; i++)
                AddUnitToHUD(deployedUnits.Dequeue());
        }

        private void AddUnitToHUD(UnitData unitData)
        {
            PlayerUnitUI playerUnitUI = playerUnits.FirstOrDefault(pUnit => pUnit.data.id == unitData.id);
            
            if (playerUnitUI == null)
            {
                var unitUI = Instantiate(unitUIPrefab, layout);
                unitUI.SetupUI(unitData);
                playerUnits.Add(unitUI);
            }
            else
            {
                playerUnitUI.AddUnit();
            }
        }

        private void RemoveUnitFromHUD(UnitData unitData)
        {
            PlayerUnitUI playerUnitUI = playerUnits.FirstOrDefault(pUnit => pUnit.data.id == unitData.id);
            if (playerUnitUI == null)
            {
                Debug.LogError("Missing? | Unit Id:" + unitData.id);
                return;
            }
            deployedUnits.Enqueue(unitData);
            playerUnitUI.RemoveUnit();
        }
    }
}