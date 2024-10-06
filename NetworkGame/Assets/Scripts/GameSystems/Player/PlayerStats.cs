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

        public int maxHp;
        public int hp;
        public int gold;
        public int groupSize;
        
        private void Awake()
        {
            i = this;
        }

        private void Start()
        {
            GameManager.i.onUpdatePlayerHp.AddListener(OnUpdatePlayerHp);
            GameManager.i.onStartGame.AddListener(OnStartGame);
        }

        private void OnStartGame()
        {
            stats = GuildManager.i.GetPlayerGuildStats();
            unitList.AddRange(UnitManager.GetStartUnits());
            onPlayerSetupComplete.Invoke();

            maxHp = GuildManager.i.startHp;
            hp = maxHp;
            gold = GuildManager.i.startGold;
            groupSize = GuildManager.i.startGroupSize;
            GuildManager.i.UpdateHp(GameManager.i.GetMyPlayerIndex(), hp);
            GuildManager.i.UpdateGroupSize(GameManager.i.GetMyPlayerIndex(), groupSize);
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
            i.gold += gold;
            i.gold = Mathf.Clamp(i.gold, 0, 99);
            
            i.onUpdateGold.Invoke();
        }
        
        public static void TakeDamage(int damage)
        {
            i.hp -= damage;
            i.onUpdateHp.Invoke();
            
            GuildManager.i.UpdateHp(GameManager.i.GetMyPlayerIndex(), i.hp);
            
            if (i.hp <= 0)
            {
                //DEATH!
            }
        }
        public static void AddGroupSize()
        {
            i.groupSize++;
            i.onAddGroupSize.Invoke();
            GuildManager.i.UpdateGroupSize(GameManager.i.GetMyPlayerIndex(), i.groupSize);
        }
        
       
        public static List<UnitData> GetUnits()
        {
            return i.unitList;
        }
        public static int GetGold()
        {
            return i.gold;
        }
        public static int GetHp()
        {
            return i.hp;
        }
        public static GuildStats GetGuildStats()
        {
            return i.stats;
        }
        public static int GetGroupSize()
        {
            return i.groupSize;
        }
        
    }
}