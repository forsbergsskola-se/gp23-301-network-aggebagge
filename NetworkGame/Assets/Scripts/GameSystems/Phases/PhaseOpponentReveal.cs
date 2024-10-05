using System.Collections;
using System.Collections.Generic;
using GameSystems.Battle;
using GameSystems.Player;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace GameSystems.Phases
{
    public class PhaseOpponentReveal : BasePhase
    {
        public Transform background;
        public TextMeshProUGUI guildText;
        public TextMeshProUGUI vsText;
        public TextMeshProUGUI opponentText;
        
        private void Start()
        {
            PlayerStats.i.onPlayerSetupComplete.AddListener(OnPlayerSetupComplete);
            BattleRoomManager.i.onOpponentsPrepared.AddListener(OnOpponentsPrepared);
        }

        private void OnPlayerSetupComplete()
        {
            guildText.text = PlayerStats.GetGuildStats().guildName;
            guildText.color = PlayerStats.GetGuildStats().guildColor;
        }

        public override void OnBeginPhase()
        {
            base.OnBeginPhase();
            
            background.gameObject.SetActive(true);
            BattleRoomManager.i.PrepareBattleOpponents();
        }

        private void OnOpponentsPrepared()
        {
            StartCoroutine(OpponentRevealAnimation());
        }

        protected override void OnEndPhase()
        {
            base.OnEndPhase();
            
            guildText.gameObject.SetActive(false);
            vsText.gameObject.SetActive(false);
            opponentText.gameObject.SetActive(false);
            background.gameObject.SetActive(false);
        }

        IEnumerator OpponentRevealAnimation()
        {
            yield return new WaitForSeconds(1);

            guildText.gameObject.SetActive(true);
            guildText.GetComponent<Animator>().Play("TextComingAtScreen", 0, 0);
            yield return new WaitForSeconds(1);
            
            vsText.gameObject.SetActive(true);
            vsText.GetComponent<Animator>().Play("TextComingAtScreen", 0, 0);
            yield return new WaitForSeconds(1);

            opponentText.text = BattleManager.i.opponent == null ? "Monster" : BattleManager.i.opponent.guildName;
            opponentText.color = BattleManager.i.opponent == null ? Color.grey : BattleManager.i.opponent.guildColor;
                
            opponentText.gameObject.SetActive(true);
            opponentText.GetComponent<Animator>().Play("TextComingAtScreen", 0, 0);
        }
        
    }
}