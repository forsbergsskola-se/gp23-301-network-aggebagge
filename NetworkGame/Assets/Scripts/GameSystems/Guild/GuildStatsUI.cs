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

        private GuildStats guildStats;
        
        private void Start()
        {
            GameManager.i.onJoinRoom.AddListener(Setup);
        }

        private void Setup()
        {
            PlayerStats.i.onAddUnit.AddListener(OnAddUnit);
            unitCount.text = PlayerStats.GetUnits().Count.ToString();

            guildStats = GameManager.i.GetPlayerStats(PhotonNetwork.LocalPlayer.ActorNumber);
            
            guildTitle.text = guildStats.guildName;
            guildTitle.color = guildStats.guildColor;
            hp.text = guildStats.hp.ToString();
            gold.text = guildStats.gold.ToString();
        }


        private void OnAddUnit(UnitSo unitSo)
        {
            unitCount.text = PlayerStats.GetUnits().Count.ToString();
        }
    }
}