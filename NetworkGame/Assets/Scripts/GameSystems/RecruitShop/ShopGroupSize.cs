using GameSystems.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.RecruitShop
{
    public class ShopGroupSize : MonoBehaviour
    {
        public int[] cost;
        public int boughtCount;

        public Button buyButton;
        public TextMeshProUGUI costText;
        private void Start()
        {
            PlayerStats.i.onPlayerSetupComplete.AddListener(UpdateButtonInteractable);
            buyButton.onClick.AddListener(OnClickButton);
            PlayerStats.i.onUpdateGold.AddListener(UpdateButtonInteractable);

            UpdateUI();
        }

        private void UpdateUI()
        {
            costText.text = cost[boughtCount].ToString();
            UpdateButtonInteractable();
        }

        private void OnClickButton()
        {
            PlayerStats.AddGold(-cost[boughtCount]);
            PlayerStats.AddGroupSize();
            boughtCount++;
            UpdateUI();
        }


        private void UpdateButtonInteractable()
        {
            if(cost.Length > boughtCount)
                buyButton.interactable = PlayerStats.GetGold() >= cost[boughtCount];
        }
    }
}