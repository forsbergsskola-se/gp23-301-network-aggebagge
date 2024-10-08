using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.Guild
{
    public class GuildListObject : MonoBehaviour
    {
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI hpText;
        public Image crest;
        
        public void SetupUI(GuildStats guildStats)
        {
            nameText.text = guildStats.guildName;
            hpText.text = guildStats.hp.ToString();
            nameText.color = guildStats.guildColor;
            GetComponent<Image>().color = new Color(guildStats.guildColor.r, guildStats.guildColor.g,
                guildStats.guildColor.b, 0.75f);

            crest.sprite = GuildManager.i.GetGuildSprite(GuildManager.i.playerGuilds.IndexOf(guildStats));
            crest.color = guildStats.guildColor;
        }
        
        public void UpdateUI(string hp)
        {
            hpText.text = hp;
        }

    }
}