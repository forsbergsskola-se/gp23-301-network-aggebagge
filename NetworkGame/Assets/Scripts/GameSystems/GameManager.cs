using System;
using System.Collections.Generic;
using GameSystems.Guild;
using GameSystems.Units;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystems
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public int playerCount;
        public int playerHp;
        public int startGold;
        public List<GuildStats> playerGuilds = new ();
        public string[] guildNames;
        
        
        // Singleton instance for easy access
        public static GameManager i;
        
        private void Awake()
        {
            if (i == null)
            {
                i = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
            
            for (int i = 0; i < playerCount; i++)
            {
                playerGuilds.Add(new GuildStats(playerHp, startGold, guildNames[i]));
            }
        }

        public void AddToPlayerHp(int playerId, int hpToAdd)
        {
            playerGuilds[playerId].hp += hpToAdd;
            playerGuilds[playerId].hp = Mathf.Clamp(playerGuilds[playerId].hp, 0, playerGuilds[playerId].maxHp);

            if (playerGuilds[playerId].hp <= 0)
            {
                //REMOVE PLAYER
            }
        }
        
        public void AddToPlayerGold(int playerId, int goldToAdd)
        {
            playerGuilds[playerId].gold += goldToAdd;
            playerGuilds[playerId].gold = Mathf.Clamp(playerGuilds[playerId].gold, 0, 99);
            
            photonView.RPC("SyncPlayerStats", RpcTarget.Others, playerId, playerGuilds[playerId].hp, playerGuilds[playerId].gold);
        }
        
        
        [PunRPC]
        void SyncPlayerStats(int playerId, int hp, int gold)
        {
            playerGuilds[playerId].hp = hp;
            playerGuilds[playerId].gold = gold;
        }
        
        // Method to get stats for a specific player
        public GuildStats GetPlayerStats(int playerId)
        {
            return playerGuilds[playerId];
        }
        
    }
}