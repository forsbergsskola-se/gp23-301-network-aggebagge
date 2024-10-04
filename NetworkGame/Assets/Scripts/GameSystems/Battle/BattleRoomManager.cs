using System.Collections.Generic;
using System.Linq;
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
        
        [HideInInspector] public UnityEvent<List<BattleRoom>> onOpponentsPrepared = new();
        private readonly List<BattleRoom> battleRooms = new();

        private void Awake()
        {
            i = this;
        }

        public class BattleRoom
        {
            public GuildStats guild1;
            public GuildStats guild2;

            public readonly List<UnitData> guild1Units = new();
            public readonly List<UnitData> guild2Units = new();

            public void SetBattleRoom(GuildStats g1, GuildStats g2)
            {
                guild1 = g1;
                guild2 = g2;
            }
            
            public void SetBattleRoom(GuildStats g)
            {
                guild1 = g;
            }

            public void SetUnits(List<UnitData> units, bool isGuild1)
            {
                SetUnits(units, isGuild1? guild1Units : guild2Units);
            }

            private void SetUnits(List<UnitData> units, List<UnitData> list)
            {
                list.Clear();

                list.AddRange(units);
            }
        }

        private void Start()
        {
            GameManager.i.onStartGame.AddListener(OnJoinRoom);
        }

        private void OnJoinRoom()
        {
            int rooms = Mathf.CeilToInt(GameManager.i.playersAlive * 0.5f);
            
            for (int r = 0; r < rooms; r++)
            {
                battleRooms.Add(new BattleRoom());
            }
        }


        public void PrepareBattleOpponents()
        {
            int player = 0;
            var guilds = GuildManager.i.playerGuilds;
            
            for (int room = 0; room < battleRooms.Count && player < GameManager.i.playersAlive; room++)
            {
                if (player < GameManager.i.playersAlive - 1)
                {
                    battleRooms[room].SetBattleRoom(guilds[player], guilds[player + 1]);
                }
                else
                {
                    battleRooms[room].SetBattleRoom(guilds[player]);
                }
                player += 2;
            }
            onOpponentsPrepared.Invoke(battleRooms);
        }
        
        public void SetPlayerUnits(List<UnitData> units, int battleRoomIndex, bool isGuild1)
        {
            photonView.RPC("SyncUnits", RpcTarget.All, units.ToArray(), battleRoomIndex, isGuild1);
        }
        
        [PunRPC]
        void SyncUnits(UnitData[] units, int battleRoomIndex, bool isGuild1)
        {
            battleRooms[battleRoomIndex].SetUnits(units.ToList(), isGuild1);
        }

        public List<UnitData> GetOpponentUnits(int index, bool isGuild1)
        {
            return isGuild1 ? battleRooms[index].guild1Units : battleRooms[index].guild2Units.ToList();
        }
        
    }
}