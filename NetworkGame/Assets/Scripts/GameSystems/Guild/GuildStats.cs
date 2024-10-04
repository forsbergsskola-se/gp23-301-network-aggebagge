using System.Collections.Generic;
using GameSystems.Units;
using UnityEngine;

namespace GameSystems.Guild
{
    [System.Serializable]
    public class GuildStats
    {
        public int playerID;
        public string guildName;
        public int maxHp;
        public int hp;
        public int gold;
        public int groupSize;
        public Color guildColor;

        public GuildStats(int id, string gName, Color color)
        {
            playerID = id;
            // maxHp = startHp;
            // hp = maxHp;
            // gold = startGold;
            guildName = gName;
            guildColor = color;
            // groupSize = startGroupSize;
        }
    }
}