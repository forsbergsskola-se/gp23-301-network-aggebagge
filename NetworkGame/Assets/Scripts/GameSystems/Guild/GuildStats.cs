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

        public GuildStats(int newMaxHp, int startGold, string gName)
        {
            maxHp = newMaxHp;
            gold = startGold;
            guildName = gName;
        }
    }
}