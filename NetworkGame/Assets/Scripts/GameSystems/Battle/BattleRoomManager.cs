using System;
using System.Collections.Generic;
using GameSystems.Guild;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystems.Battle
{
    public class BattleRoomManager : MonoBehaviourPunCallbacks
    {
        public static BattleRoomManager i;
        
        public UnityEvent<List<BattleRoom>> OnOpponentsPrepared = new();
        private List<BattleRoom> battleRooms = new();
        private int battleCount = 1;
        public int[] monsterDamage;


        private void Awake()
        {
            i = this;
        }

        public class BattleRoom
        {
            public GuildStats guild1;
            public GuildStats guild2;

            public int monsterDamage;

            public void SetBattleRoom(GuildStats g1, GuildStats g2)
            {
                guild1 = g1;
                guild2 = g2;
            }
            
            public void SetBattleRoom(GuildStats g, int mDamage)
            {
                guild1 = g;
                monsterDamage = mDamage;
            }
        }

        private void Start()
        {
            GameManager.i.onJoinRoom.AddListener(OnJoinRoom);
        }

        private void OnJoinRoom()
        {
            int rooms = Mathf.CeilToInt(GameManager.i.playerGuilds.Count * 0.5f);
            
            for (int i = 0; i < rooms; i++)
            {
                battleRooms.Add(new BattleRoom());
            }
        }


        public void PrepareBattleOpponents()
        {
            int player = 0;
            
            for (int i = 0; i < battleRooms.Count && player < GameManager.i.playerGuilds.Count; i++)
            {
                if (player < GameManager.i.playerGuilds.Count - 1)
                {
                    battleRooms[i].SetBattleRoom(GameManager.i.playerGuilds[player], GameManager.i.playerGuilds[player + 1]);
                }
                else
                {
                    battleRooms[i].SetBattleRoom(GameManager.i.playerGuilds[player], GetMonsterDamage());
                }
                player += 2;
            }
            Debug.Log("OnOpponentsPrepared");
            OnOpponentsPrepared.Invoke(battleRooms);
        }

        public void OnBattleEnd()
        {
            battleCount++;
        }

        private int GetMonsterDamage()
        {
            return monsterDamage[battleCount - 1];
        }
    }
}