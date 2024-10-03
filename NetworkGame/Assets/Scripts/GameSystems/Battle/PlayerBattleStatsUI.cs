using System;
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
        public Button addUnitButton;
        public Button readyButton;

        public TextMeshProUGUI cursedText;

        
        private void Start()
        {
            readyButton.onClick.AddListener(OnEndPrep);
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
            OnUpdateCurseUI(0, 3);
            playerDamageText.text = "0";
            opponentDamageText.text = "";
            addUnitButton.interactable = true;
            readyButton.interactable = true;
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