using System;
using System.Collections.Generic;
using GameSystems.Guild;
using GameSystems.Units;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GameSystems
{
    public class PlayerStats : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<int> onUpdateHp = new();
        [HideInInspector] public UnityEvent<int> onUpdateGold = new();
        [HideInInspector] public UnityEvent<UnitSo> onAddUnit = new();
        
        public static PlayerStats i;
        private GuildStats stats;
        
        [SerializeField] private List<UnitSo> unitList = new();
        

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
            stats = GameManager.i.GetPlayerStats(PhotonNetwork.LocalPlayer.ActorNumber);
        }

        private void OnUpdatePlayerHp(GuildStats guildStats)
        {
            if (guildStats != stats)
                return;
            
            onUpdateHp.Invoke(guildStats.hp);
        }


        public static void AddUnit(UnitSo unitSo)
        {
            i.unitList.Add(unitSo);
            i.onAddUnit.Invoke(unitSo);
        }
        
        public static void AddGold(int gold)
        {
            i.stats.gold += gold;
            i.stats.gold = Mathf.Clamp(i.stats.gold, 0, 99);
            
            i.onUpdateGold.Invoke(i.stats.gold);
        }

        public static List<UnitSo> GetUnits()
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
        
    }
}