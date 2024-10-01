using System;
using GameSystems.Units;
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

        private PlayerUnits playerUnits;
        private void Start()
        {
            playerUnits = FindObjectOfType<PlayerUnits>();

            playerUnits.onAddUnit.AddListener(OnAddUnit);
            unitCount.text = playerUnits.guildUnits.Count.ToString();
        }

        public void SetupUI(GuildStats guildStats)
        {
            guildTitle.text = guildStats.guildName;
            hp.text = guildStats.hp.ToString();
            gold.text = guildStats.gold.ToString();
        }
        
        
        private void OnAddUnit(UnitSo unitSo)
        {
            unitCount.text = playerUnits.guildUnits.Count.ToString();
        }
    }
}