using System.Collections.Generic;
using GameSystems.Units;
using UnityEngine;

namespace GameSystems.Guild
{
    [System.Serializable]
    public class GuildStats
    {
        public string guildName;
        public int maxHp;
        public int hp;


        public int gold;
        public Color guildColor;

        public GuildStats(int newMaxHp, int startGold, string gName, Color color)
        {
            maxHp = newMaxHp;
            gold = startGold;
            guildName = gName;
            guildColor = color;
        }
    }
}