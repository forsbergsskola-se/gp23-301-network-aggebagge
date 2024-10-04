using System.Collections.Generic;
using GameSystems.Guild;
using GameSystems.Units;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystems.Player
{
    public class PlayerStats : MonoBehaviour
    {
        [HideInInspector] public UnityEvent onPlayerSetupComplete = new();
        [HideInInspector] public UnityEvent onUpdateHp = new();
        [HideInInspector] public UnityEvent onUpdateGold = new();
        [HideInInspector] public UnityEvent onAddUnit = new();
        [HideInInspector] public UnityEvent onAddGroupSize = new();

        public static PlayerStats i;
        private GuildStats stats;
        
        [SerializeField] private readonly List<UnitData> unitList = new();
        

        private void Awake()
        {
            i = this;
        }

        private void Start()
        {
            GameManager.i.onUpdatePlayerHp.AddListener(OnUpdatePlayerHp);
            GameManager.i.onJoinRoom.AddListener(OnJoinRoom);
        }

        private void OnJoinRoom()
        {
            stats = GuildManager.i.GetPlayerStats();
            unitList.AddRange(UnitManager.GetStartUnits());
            onPlayerSetupComplete.Invoke();
        }

        private void OnUpdatePlayerHp(GuildStats guildStats)
        {
            if (guildStats != stats)
                return;
            
            onUpdateHp.Invoke();
        }


        public static void AddUnit(UnitData unitData)
        {
            i.unitList.Add(unitData);
            i.onAddUnit.Invoke();
        }
        
        public static void AddGold(int gold)
        {
            i.stats.gold += gold;
            i.stats.gold = Mathf.Clamp(i.stats.gold, 0, 99);
            
            i.onUpdateGold.Invoke();
        }
        
        public static void TakeDamage(int damage)
        {
            i.stats.hp -= damage;
            i.onUpdateHp.Invoke();

            if (i.stats.hp <= 0)
            {
                //DEATH!
            }
        }
        public static void AddGroupSize()
        {
            i.stats.groupSize++;
            i.onAddGroupSize.Invoke();
        }
        
       
        public static List<UnitData> GetUnits()
        {
            return i.unitList;
        }
        public static int GetGold()
        {
            return i.stats.gold;
        }
        public static int GetHp()
        {
            return i.stats.hp;
        }
        public static GuildStats GetGuildStats()
        {
            return i.stats;
        }
        public static int GetGroupSize()
        {
            return i.stats.groupSize;
        }
        
    }
}