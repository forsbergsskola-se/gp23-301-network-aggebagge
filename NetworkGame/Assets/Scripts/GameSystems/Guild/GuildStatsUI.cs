using System;
using GameSystems.Units;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace GameSystems.Guild
{
    public class GuildStatsUI : MonoBehaviour
    {
        public TextMeshProUGUI guildTitle;
        public TextMeshProUGUI hp;
        public TextMeshProUGUI gold;
        public TextMeshProUGUI unitCount;

        private GuildUnitManager guildUnitManager;
        private GuildStats guildStats;
        
        private void Start()
        {
            GameManager.i.onJoinRoom.AddListener(Setup);
        }

        private void Setup()
        {
            guildUnitManager = FindObjectOfType<GuildUnitManager>();
            guildUnitManager.onAddUnit.AddListener(OnAddUnit);
            unitCount.text = guildUnitManager.guildUnits.Count.ToString();

            guildStats = GameManager.i.GetPlayerStats(PhotonNetwork.LocalPlayer.ActorNumber);
            
            guildTitle.text = guildStats.guildName;
            hp.text = guildStats.hp.ToString();
            gold.text = guildStats.gold.ToString();
        }


        private void OnAddUnit(UnitSo unitSo)
        {
            unitCount.text = guildUnitManager.guildUnits.Count.ToString();
        }
    }
}