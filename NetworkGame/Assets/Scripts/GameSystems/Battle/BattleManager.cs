using System;
using System.Collections.Generic;
using System.Linq;
using GameSystems.Guild;
using GameSystems.Player;
using GameSystems.Units;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.Battle
{
    public class BattleManager : MonoBehaviour
    {
        public BattleFieldUI playerBattleField;
        public BattleFieldUI enemyBattleField;
        public Button addUnitButton;

        private Queue<UnitSo> unitQueue = new();
        private System.Random rng;
        
        private void Start()
        {
            BattleRoomManager.i.OnOpponentsPrepared.AddListener(SetupBattle);
            addUnitButton.onClick.AddListener(OnClickAddUnit);
            
            rng = new System.Random();
        }

        private void OnClickAddUnit()
        {
            playerBattleField.AddUnit(unitQueue.Dequeue());

            if (playerBattleField.battleUnits.Count == playerBattleField.slotCount || unitQueue.Count == 0)
            {
                addUnitButton.interactable = false;
            }
        }


        private void CreateUnitQueue()
        {
            List<UnitSo> units = new List<UnitSo>(PlayerStats.GetUnits());
            ShuffleList(units);

            unitQueue = new Queue<UnitSo>(units);
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
        

        private void SetupBattle(List<BattleRoomManager.BattleRoom> battleRooms)
        {
            CreateUnitQueue();
            addUnitButton.interactable = true;
            
            GuildStats opponentGuildStats = GetOpponentGuildStats(battleRooms);
            playerBattleField.SetupSlots(PlayerStats.GetGroupSize());
            
            if(opponentGuildStats != null)
                enemyBattleField.SetupSlots(opponentGuildStats.groupSize);
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