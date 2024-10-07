using System;
using GameSystems.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.Battle
{
    public class PlayerBattleStatsUI : MonoBehaviour
    {
        public Transform curseLayout;
        public TextMeshProUGUI playerDamageText;
        public TextMeshProUGUI opponentDamageText;
        
        public TextMeshProUGUI yourGuildText;
        public TextMeshProUGUI opponentGuildText;
        public TextMeshProUGUI winGoldText;
        public TextMeshProUGUI loseHpText;

        public Button addUnitButton;
        public Button readyButton;

        public TextMeshProUGUI cursedText;

        
        private void Start()
        {
            readyButton.onClick.AddListener(OnEndPrep);
            
            yourGuildText.text = PlayerStats.GetGuildStats().guildName;
            yourGuildText.color = PlayerStats.GetGuildStats().guildColor;
        }

        private void Update()
        {
            if (addUnitButton.interactable)
            {
                if(Input.GetKeyUp(KeyCode.D))
                    addUnitButton.onClick.Invoke();
            }
            if (readyButton.interactable)
            {
                if(Input.GetKeyUp(KeyCode.R))
                    readyButton.onClick.Invoke();
            }
        }

        public void OnUpdatePlayerDamage(int damage)
        {
            playerDamageText.text = damage.ToString();
        }

        public void OnUpdateOpponentDamage(int damage)
        {
            opponentDamageText.text = damage.ToString();
        }

        public void OnUpdateCurseUI(int curses, int curseSlots)
        {
            for (int i = 0; i < curseLayout.childCount; i++)
            {
                var curseUI = curseLayout.GetChild(i).GetComponent<BattleCurseUI>();
                curseUI.gameObject.SetActive(i < curseSlots);
                curseUI.SetCurse(i < curses);
            }
        }

        public void ResetBattleUI()
        {
            cursedText.gameObject.SetActive(false);
            OnUpdateCurseUI(0, 3);
            playerDamageText.text = "0";
            opponentDamageText.text = "";
            addUnitButton.interactable = true;
            readyButton.interactable = true;

            
            opponentGuildText.text = BattleManager.i.opponent != null ? BattleManager.i.opponent.guildName : "Monster";
            opponentGuildText.color = BattleManager.i.opponent != null ? BattleManager.i.opponent.guildColor : Color.gray;

            winGoldText.text = BattleManager.i.GetBattleStats().winGold.ToString();
            loseHpText.text = BattleManager.i.GetBattleStats().loseDamage.ToString();
        }

        public void OnCursed()
        {
            cursedText.gameObject.SetActive(true);
            OnEndPrep();
        }

        private void OnEndPrep()
        {
            addUnitButton.interactable = false;
            readyButton.interactable = false;
            BattleManager.i.EndPlayerPrep();
        }
        
    }
}