using GameSystems.Guild;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameSystems.Player
{
    public class PlayerStatsHUD : MonoBehaviour
    {
        public TextMeshProUGUI guildTitle;
        public TextMeshProUGUI hp;
        public TextMeshProUGUI gold;
        public TextMeshProUGUI groupSize;

        private GuildStats guildStats;
        
        private void Start()
        {
            PlayerStats.i.onPlayerSetupComplete.AddListener(Setup);
        }

        private void Setup()
        {
            guildStats = GuildManager.i.GetPlayerStats();
            
            guildTitle.text = guildStats.guildName;
            guildTitle.color = guildStats.guildColor;
            
            PlayerStats.i.onUpdateGold.AddListener(UpdateUI);
            PlayerStats.i.onUpdateHp.AddListener(UpdateUI);
            PlayerStats.i.onAddGroupSize.AddListener(UpdateUI);

            UpdateUI();
        }

        private void UpdateUI()
        {
            hp.text = guildStats.hp.ToString();
            gold.text = guildStats.gold.ToString();
            groupSize.text = guildStats.groupSize.ToString();
        }
    }
}