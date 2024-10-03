using System;
using System.Collections.Generic;
using System.Linq;
using GameSystems.Guild;
using GameSystems.Player;
using GameSystems.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.Battle
{
    public class BattleManager : MonoBehaviour
    {
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
        
        

        public void OnEndPlayerPrep()
        {
            
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