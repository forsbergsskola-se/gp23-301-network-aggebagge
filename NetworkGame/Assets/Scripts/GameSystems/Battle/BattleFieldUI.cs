using System;
using System.Collections.Generic;
using GameSystems.Units;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GameSystems.Battle
{
    public class BattleFieldUI : MonoBehaviour
    {
        public Transform layout;
        public BattleUnit battleUnitPrefab;
        public GameObject fieldSlotPrefab;

        public List<BattleUnit> units = new();
        
        private Queue<GameObject> fieldSlots = new ();
        
        public AudioSource curseAudio;
        public AudioSource antiCurseAudio;

        private void Start()
        {
            BattleManager.i.onPlayerEndBattle.AddListener(OnEndBattle);
        }

        private void OnEndBattle()
        {
            fieldSlots.Clear();
            units.Clear();
            
            foreach (Transform child in layout)
                Destroy(child.gameObject);
        }

        public void SetupSlots(int slots)
        {
            for (int i = 0; i < slots; i++)
            {
                var fieldSlot = Instantiate(fieldSlotPrefab, layout);
                fieldSlots.Enqueue(fieldSlot);
            }
        }

        public BattleUnit AddUnit(UnitData unitData, bool isPlayer)
        {
            var battleUnit = Instantiate(battleUnitPrefab, fieldSlots.Dequeue().transform);
            battleUnit.SetupUI(unitData);
            if(isPlayer)
                battleUnit.RemoveAction();
            
            battleUnit.onKill.AddListener(OnKillUnit);
            units.Add(battleUnit);
            
            if(unitData.attributeType == AttributeType.Curse)
                curseAudio.Play();
            else if(unitData.attributeType == AttributeType.AntiCurse)
                antiCurseAudio.Play();
            
            return battleUnit;
        }

        private void OnKillUnit(BattleUnit battleUnit)
        {
            units.Remove(battleUnit);
            BattleManager.i.playerBattleStats.battleUnits.Remove(battleUnit);
            BattleManager.i.playerBattleStats.AddDamage(-battleUnit.data.damage);
        }

    }
}