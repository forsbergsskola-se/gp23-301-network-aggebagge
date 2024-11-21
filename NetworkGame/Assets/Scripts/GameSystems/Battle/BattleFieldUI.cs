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
        
        private List<GameObject> fieldSlots = new ();
        
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
                fieldSlots.Add(fieldSlot);
            }
        }

        public BattleUnit AddUnit(UnitData unitData, bool isPlayer)
        {
            var battleUnit = Instantiate(battleUnitPrefab, fieldSlots[0].transform);
            fieldSlots.RemoveAt(0);
            battleUnit.SetupUI(unitData);
            if(!isPlayer)
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
            fieldSlots.Insert(0, battleUnit.transform.parent.gameObject);
            units.Remove(battleUnit);
            
            BattleManager.i.playerBattleStats.RemoveUnit(battleUnit);
        }

    }
}