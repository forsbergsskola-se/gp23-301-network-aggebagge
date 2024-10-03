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
        

        public static BattleManager i;

        private void Awake()
        {
            i = this;
        }

        private void Start()
        {
            BattleRoomManager.i.OnOpponentsPrepared.AddListener(SetupBattle);
        }

     

        private void SetupBattle(List<BattleRoomManager.BattleRoom> battleRooms)
        {
            playerBattleStats.SetupNewBattle();
                
            GuildStats opponentGuildStats = GetOpponentGuildStats(battleRooms);
            playerBattleField.SetupSlots(PlayerStats.GetGroupSize());
            
            if(opponentGuildStats != null)
                enemyBattleField.SetupSlots(opponentGuildStats.groupSize);
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