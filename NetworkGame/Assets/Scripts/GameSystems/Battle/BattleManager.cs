using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameSystems.Guild;
using GameSystems.Phases;
using GameSystems.Player;
using GameSystems.Units;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameSystems.Battle
{
    public class BattleManager : MonoBehaviour
    {
        [HideInInspector] public UnityEvent onPlayerEndPrep = new();
        
        public BattleFieldUI playerBattleField;
        public BattleFieldUI enemyBattleField;
        public PlayerBattleStats playerBattleStats;

        [HideInInspector]public GuildStats opponent;
        public static BattleManager i;

        private void Awake()
        {
            i = this;
        }

        private void Start()
        {
            BattleRoomManager.i.OnOpponentsPrepared.AddListener(GetOpponent);
        }

     

        private void GetOpponent(List<BattleRoomManager.BattleRoom> battleRooms)
        {
            opponent = GetOpponentGuildStats(battleRooms);
        }

        public void SetupBattle()
        {
            playerBattleStats.SetupNewBattle();
            playerBattleField.SetupSlots(PlayerStats.GetGroupSize());
            
            if(opponent != null)
                enemyBattleField.SetupSlots(opponent.groupSize);
        }
        
        

        public void EndPlayerPrep()
        {
            if(playerBattleStats.isCursed)
                onPlayerEndPrep.Invoke();
            else
                StartCoroutine(AnimateBonuses());
        }

        private IEnumerator AnimateBonuses()
        {
            float animationWaitTime = 0.25f;

            yield return new WaitForSeconds(1);
            
            //loop and animate each bonus
            foreach (BattleUnit battleUnit in playerBattleStats.battleUnits)
            {
                if (battleUnit.unit.goldGain > 0)
                {
                    battleUnit.PopupText(false, battleUnit.unit.goldGain);
                    yield return new WaitForSeconds(animationWaitTime);
                }
            }
            
            yield return new WaitForSeconds(1);
            onPlayerEndPrep.Invoke();
        }
        

        private GuildStats GetOpponentGuildStats(List<BattleRoomManager.BattleRoom> battleRooms)
        {
            GuildStats playerGuildStats = PlayerStats.GetGuildStats();

            foreach (var battleRoom in battleRooms)
            {
                if (battleRoom.guild1 == playerGuildStats)
                {
                    return battleRoom.guild2;
                }
                if (battleRoom.guild2 == playerGuildStats)
                {
                    return battleRoom.guild1;
                }
            }

            return null;
        }
    }
}