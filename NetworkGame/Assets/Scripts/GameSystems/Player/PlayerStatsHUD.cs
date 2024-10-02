using GameSystems.Guild;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace GameSystems.Player
{
    public class PlayerStatsHUD : MonoBehaviour
    {
        public TextMeshProUGUI guildTitle;
        public TextMeshProUGUI hp;
        public TextMeshProUGUI gold;
        public TextMeshProUGUI unitCount;

        private GuildStats guildStats;
        
        private void Start()
        {
            PlayerStats.i.onPlayerSetupComplete.AddListener(Setup);
        }

        private void Setup()
        {
            unitCount.text = PlayerStats.GetUnits().Count.ToString();

            guildStats = GameManager.i.GetPlayerStats(PhotonNetwork.LocalPlayer.ActorNumber);
            
            guildTitle.text = guildStats.guildName;
            guildTitle.color = guildStats.guildColor;
            
            
            PlayerStats.i.onAddUnit.AddListener(UpdateUI);
            PlayerStats.i.onUpdateGold.AddListener(UpdateUI);
            PlayerStats.i.onUpdateHp.AddListener(UpdateUI);
            
            UpdateUI();
        }

        private void UpdateUI()
        {
            hp.text = guildStats.hp.ToString();
            gold.text = guildStats.gold.ToString();
            unitCount.text = PlayerStats.GetUnits().Count.ToString();
        }
    }
}