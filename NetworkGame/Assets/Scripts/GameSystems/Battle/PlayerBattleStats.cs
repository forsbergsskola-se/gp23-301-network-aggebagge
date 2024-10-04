using System;
using System.Collections.Generic;
using GameSystems.Player;
using GameSystems.Units;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GameSystems.Battle
{
    public class PlayerBattleStats : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<BattleUnit> onAddUnit = new ();
        
        private int damage;
        private int curses;
        private int curseSlots;
        private int unitSlots;
        

        [HideInInspector] public List<BattleUnit> battleUnits = new ();
        [HideInInspector] public Queue<UnitData> unitQueue = new ();

        private PlayerBattleStatsUI battleStatsUI;
        private System.Random rng;


        [HideInInspector]  public bool isCursed;

        private void Awake()
        {
            rng = new System.Random();
            battleStatsUI = GetComponent<PlayerBattleStatsUI>();
            battleStatsUI.addUnitButton.onClick.AddListener(AddUnit);
        }


        public void SetupNewBattle()
        {
            damage = 0;
            isCursed = false;
            curses = 0;
            curseSlots = 3;
            unitSlots = PlayerStats.GetGroupSize();
            battleUnits.Clear();
            battleStatsUI.ResetBattleUI();
            CreateUnitQueue();
        }


        private void AddDamage(int dmg)
        {
            damage += dmg;
            battleStatsUI.OnUpdatePlayerDamage(damage);
        }
        
        private void AddCurseSlot()
        {
            curseSlots++;
            battleStatsUI.OnUpdateCurseUI(curses, curseSlots);
        }

        private void AddCurse()
        {
            curses++;
            battleStatsUI.OnUpdateCurseUI(curses, curseSlots);
            
            if (curses == curseSlots)
            {
                OnCursed();
            }
        }

        private void OnCursed()
        {
            battleStatsUI.OnCursed();
            isCursed = true;

            // foreach (var battleUnit in battleUnits)
            //     Destroy(battleUnit.gameObject);
            
            damage = 0;
            battleStatsUI.OnUpdatePlayerDamage(damage);
        }
        
        
        private void AddUnit()
        {
            UnitData unit = unitQueue.Dequeue();
            var battleUnit = BattleManager.i.playerBattleField.AddUnit(unit);
            
            battleUnits.Add(battleUnit);
            
            if (battleUnits.Count == unitSlots || unitQueue.Count == 0)
                battleStatsUI.addUnitButton.interactable = false;

            if (battleUnit.data.damage > 0)
                AddDamage(battleUnit.data.damage);
            
            
            if (battleUnit.data.attributeType != AttributeType.None)
            {
                if (battleUnit.data.attributeType == AttributeType.Curse)
                    AddCurse();
                else if (battleUnit.data.attributeType == AttributeType.AntiCurse)
                    AddCurseSlot();
            }
        }
        
        
        private void CreateUnitQueue()
        {
            List<UnitData> units = new List<UnitData>(PlayerStats.GetUnits());
            ShuffleList(units);

            unitQueue = new Queue<UnitData>(units);
        }

        void ShuffleList<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]); // Cleaner tuple swap
            }
            
        }

        public int GetDamage()
        {
            return damage;
        }
        
    }
}