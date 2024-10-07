using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameSystems.Guild;
using GameSystems.Player;
using GameSystems.Units;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
        public Image monster;
        
        private List<UnitData> opponentUnits;

        public TextMeshProUGUI resultText;

        private int battleCount;
        public BattleStats[] battleStats;

        public AudioClip winClip;
        public AudioClip loseClip;
        public AudioClip drawClip;
        public AudioSource battleResultAudio;

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
            monster.gameObject.SetActive(opponent == null);
        }

        public void SetupBattleField()
        {
            opponentDamageText.text = "";
            opponentCursedText.gameObject.SetActive(false);
            resultText.gameObject.SetActive(false);
            
            playerBattleStats.SetupNewBattle();
            playerBattleField.SetupSlots(PlayerStats.GetGroupSize());
        }
        
        

        public void EndPlayerPrep()
        {
            List<UnitData> dataList = new();
            foreach (var unit in playerBattleStats.battleUnits)
                dataList.Add(unit.data);
            
            BattleRoomManager.i.PlayerEndBattle(dataList);
            onPlayerEndPrep.Invoke();
        }

        public void OnBattlePhaseBegin()
        {
            if (opponent != null)
            {
                Debug.Log(opponent);
                opponentUnits = BattleRoomManager.i.GetOpponentUnits();
                enemyBattleField.SetupSlots(opponentUnits.Count);
            }

            StartCoroutine(AnimateBonuses());
        }

        private int FullPartyBonus(int groupSize, List<BattleUnit> units)
        {
            if(groupSize > units.Count)
                return 0;
            
            var leaders =
                units.Where(unit => unit.data.attributeType == AttributeType.FullParty);

            if (!leaders.Any())
                return 0;

            int damage = 0;
            foreach (var battleUnit in leaders)
            {
                battleUnit.PopupText(true, 4);
                damage += 4;
            }
            return damage;
        }

        private int VikingBonus(List<BattleUnit> units)
        {
            var vikings =
                units.Where(unit => unit.data.attributeType == AttributeType.Viking);

            foreach (var viking in vikings)
                viking.PopupIcon(UnitManager.GetUnitSo(viking.data.id).attribute.icon);
            
            switch (vikings.Count())
            {
                case 0: return 0;
                case 1: return 1;
                case 2: return 2;
                case 3: return 3;
                case 4: return 4;
                default: return 4;
            }
        }


        private bool RoyaltyBonus()
        {
            var queens =
                playerBattleStats.battleUnits.Where(unit => unit.data.attributeType == AttributeType.Royalty);
            
            if (!queens.Any())
                return false;
            
            int adventurerCount = playerBattleStats.battleUnits.Count(unit => unit.data.id == 1);
            
            foreach (var battleUnit in queens)
            {
                battleUnit.PopupText(false, adventurerCount);
                PlayerStats.AddGold(adventurerCount);
            }
            return true;
        }
        
        
        private IEnumerator AnimateBonuses()
        {
            float animationWaitTime = 0.25f;

            yield return new WaitForSeconds(0.5f);
            
            if (!playerBattleStats.isCursed)
            {
                //loop and animate each bonus
                foreach (var battleUnit in playerBattleStats.battleUnits)
                {
                    if (battleUnit.data.goldGain > 0)
                    {
                        PlayerStats.AddGold(battleUnit.data.goldGain);
                        battleUnit.PopupText(false, battleUnit.data.goldGain);
                        yield return new WaitForSeconds(animationWaitTime);
                    }
                }
                yield return new WaitForSeconds(1.5f);

                int fullPartyBonus = FullPartyBonus(PlayerStats.GetGroupSize(), playerBattleField.units);
                if (fullPartyBonus > 0)
                {
                    playerBattleStats.AddDamage(fullPartyBonus);
                    yield return new WaitForSeconds(1.5f);
                }

                int vikingBonus = VikingBonus(playerBattleField.units);
                if (vikingBonus > 0)
                {
                    playerBattleStats.AddDamage(vikingBonus);
                    yield return new WaitForSeconds(1.5f);
                }
                if(RoyaltyBonus())
                    yield return new WaitForSeconds(1.5f);
            }
            
            int opponentDamage = 0;
            int opponentCurses = 0;
            var battleStat = battleStats[battleCount];

            if (opponent != null)
            {
                foreach (var unitSo in opponentUnits)
                {
                    enemyBattleField.AddUnit(unitSo, false);
                    opponentDamage += unitSo.damage;
                    opponentDamageText.text = opponentDamage.ToString();
                    if (unitSo.attributeType == AttributeType.Curse)
                    {
                        opponentCurses++;
                        if (opponentCurses == 3)
                        {
                            opponentCursedText.gameObject.SetActive(true);
                            opponentDamage = 0;
                        }
                    }
                    else if (unitSo.attributeType == AttributeType.AntiCurse)
                        opponentCurses--;
                    
                    yield return new WaitForSeconds(animationWaitTime);
                }
                yield return new WaitForSeconds(1.5f);

                if (opponentCurses < 3)
                {
                    int fullPartyBonus = FullPartyBonus(opponent.groupSize, enemyBattleField.units);
                    if (fullPartyBonus > 0)
                    {
                        opponentDamage += fullPartyBonus;
                        opponentDamageText.text = opponentDamage.ToString();
                        yield return new WaitForSeconds(1.5f);
                    }
                    int vikingBonus = VikingBonus(enemyBattleField.units);
                    if (vikingBonus > 0)
                    {
                        opponentDamage += vikingBonus;
                        opponentDamageText.text = opponentDamage.ToString();
                        yield return new WaitForSeconds(1.5f);
                    }

                }
            }
            else
            {
                opponentDamage = battleStat.monsterDamage;
                opponentDamageText.text = opponentDamage.ToString();
                yield return new WaitForSeconds(1.5f);
            }
            
            SoundtrackManager.StopMusic(Soundtrack.Battle);
            
            bool isWin = playerBattleStats.GetDamage() > opponentDamage;
            bool isLose = playerBattleStats.GetDamage() < opponentDamage;

            resultText.text = isWin? "<size=150%>Winner</size>\n+" + battleStat.winGold + " Gold" : isLose? "<size=150%>Loser</size>\n-" + battleStat.loseDamage + " Hp" : "<size=150%>Draw</size>";
            resultText.gameObject.SetActive(true);

            battleResultAudio.clip = isWin ? winClip : isLose ? loseClip : drawClip;
            battleResultAudio.Play();
            
            yield return new WaitForSeconds(1);
            
            if(isWin)
                PlayerStats.AddGold(battleStat.winGold);
            else if (isLose)
                PlayerStats.TakeDamage(battleStat.loseDamage);
            
            yield return new WaitForSeconds(3);

            battleCount++;
            if(GuildManager.i.GetPlayerGuildStats().hp > 0)
                onPlayerEndBattle.Invoke();
        }
        

        public BattleStats GetBattleStats()
        {
            return battleStats[battleCount];
        }
    }
}