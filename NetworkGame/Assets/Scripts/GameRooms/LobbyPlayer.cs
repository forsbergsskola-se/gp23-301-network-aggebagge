using GameSystems;
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
        
        public void SetupValues(GuildStats guildStats)
        {
            guildText.text = guildStats.guildName;
            guildText.color = guildStats.guildColor;
            guildCrest.color = guildStats.guildColor;
            
            guildCrest.sprite = GuildManager.i.GetGuildSprite(GuildManager.i.playerGuilds.IndexOf(guildStats));
        }
    }
}