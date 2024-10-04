using System;
using System.Collections.Generic;
using GameSystems.Guild;
using GameSystems.Units;
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

        private void Awake()
        {
            i = this;
        }

        public class BattleRoom
        {
            public GuildStats guild1;
            public GuildStats guild2;

            public List<UnitSo> guild1Units = new();
            public List<UnitSo> guild2Units = new();

            public void SetBattleRoom(GuildStats g1, GuildStats g2)
            {
                guild1 = g1;
                guild2 = g2;
            }
            
            public void SetBattleRoom(GuildStats g)
            {
                guild1 = g;
            }

            public void SetUnits(List<BattleUnit> units, bool isGuild1)
            {
                SetUnits(units, isGuild1? guild1Units : guild2Units);
            }

            private void SetUnits(List<BattleUnit> units, List<UnitSo> list)
            {
                list.Clear();
                
                foreach (var battleUnit in units)
                    list.Add(battleUnit.unit);
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
                    battleRooms[i].SetBattleRoom(GameManager.i.playerGuilds[player]);
                }
                player += 2;
            }
            OnOpponentsPrepared.Invoke(battleRooms);
        }
        
        public void SetPlayerUnits(List<BattleUnit> units, int battleRoomIndex, bool isGuild1)
        {
            photonView.RPC("SyncUnits", RpcTarget.All, units, battleRoomIndex, isGuild1);
        }

        public List<UnitSo> GetOpponentUnits(int index, bool isGuild1)
        {
            return isGuild1 ? battleRooms[index].guild1Units : battleRooms[index].guild2Units;
        }
        
        [PunRPC]
        void SyncUnits(List<BattleUnit> units, int battleRoomIndex, bool isGuild1)
        {
            battleRooms[battleRoomIndex].SetUnits(units, isGuild1);
        }

        
    }
}