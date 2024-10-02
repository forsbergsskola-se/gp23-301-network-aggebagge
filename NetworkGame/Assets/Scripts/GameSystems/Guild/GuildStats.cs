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
        public int groupSize;

        public int gold;
        public Color guildColor;

        public GuildStats(int startHp, int startGold, int startGroupSize, string gName, Color color)
        {
            maxHp = startHp;
            hp = maxHp;
            gold = startGold;
            guildName = gName;
            guildColor = color;
            groupSize = startGroupSize;
        }
    }
}