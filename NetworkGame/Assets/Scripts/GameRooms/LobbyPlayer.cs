using GameSystems.Guild;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameRooms
{
    public class LobbyPlayer : MonoBehaviour
    {
        public TextMeshProUGUI guildText;
        public Image guildCrest;
        public int id;

        private GuildStats stats;
        
        public void SetupValues(GuildStats guildStats)
        {
            stats = guildStats;
            id = guildStats.playerID;
            
            guildText.text = guildStats.guildName;
            guildText.color = guildStats.guildColor;
            guildCrest.color = guildStats.guildColor;
        }
    }
}