using TMPro;
using UnityEngine;

namespace GameSystems.Player
{
    public class PlayerStatsHUD : MonoBehaviour
    {
        public TextMeshProUGUI guildTitle;
        public TextMeshProUGUI hp;
        public TextMeshProUGUI gold;
        public TextMeshProUGUI groupSize;

        private void Start()
        {
            PlayerStats.i.onPlayerSetupComplete.AddListener(Setup);
        }

        private void Setup()
        {
            var guildStats = PlayerStats.GetGuildStats();
            guildTitle.text = guildStats.guildName;
            guildTitle.color = guildStats.guildColor;
            
            PlayerStats.i.onUpdateGold.AddListener(UpdateUI);
            PlayerStats.i.onUpdateHp.AddListener(UpdateUI);
            PlayerStats.i.onAddGroupSize.AddListener(UpdateUI);

            UpdateUI();
        }

        private void UpdateUI()
        {
            hp.text = PlayerStats.i.hp.ToString();
            gold.text = PlayerStats.i.gold.ToString();
            groupSize.text = PlayerStats.i.groupSize.ToString();
        }
    }
}