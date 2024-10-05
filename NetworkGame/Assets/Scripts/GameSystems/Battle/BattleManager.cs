using System;
using System.Collections;
using System.Collections.Generic;
using GameSystems.Guild;
using GameSystems.Player;
using GameSystems.Units;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystems.Battle
{
    public class BattleManager : MonoBehaviour
    {
        [HideInInspector] public UnityEvent onPlayerEndPrep = new();
        [HideInInspector] public UnityEvent onPlayerEndBattle = new();

        public BattleFieldUI playerBattleField;
        public BattleFieldUI enemyBattleField;
        public PlayerBattleStats playerBattleStats;

        public static BattleManager i;

        private bool isGuild1;
        private int battleRoomIndex;
        
        [HideInInspector]public GuildStats opponent;
        public TextMeshProUGUI opponentDamageText;
        public TextMeshProUGUI opponentCursedText;

        private List<UnitData> opponentUnits;

        public TextMeshProUGUI resultText;

        private int battleCount;
        public BattleStats[] battleStats;
        
        [Serializable]
        public class BattleStats
        {
            public int loseDamage;
            public int winGold;
            public int monsterDamage;
        }
        
        private void Awake()
        {
            i = this;
        }

        private void Start()
        {
            BattleRoomManager.i.onOpponentsPrepared.AddListener(GetOpponent);
        }

     

        private void GetOpponent()
        {
            opponent =  BattleRoomManager.i.GetOpponentGuildStats();
            Debug.Log(opponent.guildName);
        }

        public void SetupBattleField()
        {
            battleCount++;
            opponentDamageText.text = "";
            opponentCursedText.gameObject.SetActive(false);
            resultText.gameObject.SetActive(false);
            
            playerBattleStats.SetupNewBattle();
            playerBattleField.SetupSlots(PlayerStats.GetGroupSize());
        }
        
        

        public void EndPlayerPrep()
        {
            onPlayerEndPrep.Invoke();
            
            List<UnitData> dataList = new();
            foreach (var unit in playerBattleStats.battleUnits)
                dataList.Add(unit.data);
            
            BattleRoomManager.i.PlayerEndBattle(dataList);
        }

        public void OnBattlePhaseBegin()
        {
            if (opponent != null)
            {
                opponentUnits = BattleRoomManager.i.GetOpponentUnits();
                enemyBattleField.SetupSlots(opponentUnits.Count);
            }

            StartCoroutine(AnimateBonuses());
        }

        private IEnumerator AnimateBonuses()
        {
            float animationWaitTime = 0.25f;

            yield return new WaitForSeconds(0.5f);
            
            if (!playerBattleStats.isCursed)
            {
                //loop and animate each bonus
                foreach (var unitData in playerBattleStats.battleUnits)
                {
                    if (unitData.data.goldGain > 0)
                    {
                        PlayerStats.AddGold(unitData.data.goldGain);
                        unitData.PopupText(false, unitData.data.goldGain);
                        yield return new WaitForSeconds(animationWaitTime);
                    }
                }
                
                yield return new WaitForSeconds(1);
            }
            
            int opponentDamage = 0;
            int opponentCurses = 0;
            var battleStat = battleStats[battleCount];

            if (opponent != null)
            {
                foreach (var unitSo in opponentUnits)
                {
                    enemyBattleField.AddUnit(unitSo);
                    opponentDamage += unitSo.damage;
                    opponentDamageText.text = opponentDamage.ToString();
                    if (unitSo.attributeType == AttributeType.Curse)
                    {
                        opponentCurses++;
                        if (opponentCurses == 3)
                            opponentCursedText.gameObject.SetActive(true);
                    }
                    else if (unitSo.attributeType == AttributeType.AntiCurse)
                        opponentCurses--;
                    
                    yield return new WaitForSeconds(animationWaitTime);
                }
                
                yield return new WaitForSeconds(1);
            }
            else
            {
                opponentDamage = battleStat.monsterDamage;
            }

            Debug.Log(playerBattleStats.GetDamage() + " " +  opponentDamage);
            bool isWin = playerBattleStats.GetDamage() > opponentDamage;
            bool isLose = playerBattleStats.GetDamage() < opponentDamage;

            resultText.text = isWin? "<size=150%>Winner</size>\n+" + battleStat.winGold + " Gold" : isLose? "<size=150%>Loser</size>\n-" + battleStat.loseDamage + " Hp" : "<size=150%>Draw</size>";
            resultText.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(1);
            
            if(isWin)
                PlayerStats.AddGold(battleStat.winGold);
            else if (isLose)
                PlayerStats.TakeDamage(battleStat.loseDamage);
            
            yield return new WaitForSeconds(2);

            onPlayerEndBattle.Invoke();
        }
        

        // private GuildStats GetOpponentGuildStats(List<BattleRoomManager.BattleRoom> battleRooms)
        // {
        //     GuildStats playerGuildStats = PlayerStats.GetGuildStats();
        //
        //     foreach (var battleRoom in battleRooms)
        //     {
        //         if (battleRoom.guild1 == playerGuildStats)
        //         {
        //             battleRoomIndex = battleRooms.IndexOf(battleRoom);
        //             isGuild1 = true;
        //             return battleRoom.guild2;
        //         }
        //         if (battleRoom.guild2 == playerGuildStats)
        //         {
        //             battleRoomIndex = battleRooms.IndexOf(battleRoom);
        //             isGuild1 = false;
        //             return battleRoom.guild1;
        //         }
        //     }
        //
        //     return null;
        // }

        public BattleStats GetBattleStats()
        {
            return battleStats[battleCount];
        }
    }
}