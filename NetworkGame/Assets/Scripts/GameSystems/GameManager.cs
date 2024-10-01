using System;
using System.Collections.Generic;
using GameSystems.Guild;
using GameSystems.Units;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystems
{
    public class GameManager : MonoBehaviour
    {
        public int playerCount;
        public int playerHp;
        public int startGold;
        public List<GuildStats> playerGuilds = new ();
        public string[] guildNames;
        
        private void Awake()
        {
            for (int i = 0; i < playerCount; i++)
            {
                playerGuilds.Add(new GuildStats(playerHp, startGold, guildNames[i]));
            }
        }
    }
}