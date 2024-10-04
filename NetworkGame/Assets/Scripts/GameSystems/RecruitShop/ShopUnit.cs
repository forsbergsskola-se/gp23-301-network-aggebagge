using System;
using GameSystems.Player;
using GameSystems.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.RecruitShop
{
    public class ShopUnit : MonoBehaviour
    {
        public TextMeshProUGUI costText;
        public UnitUI unitUI;
        private UnitData data;

        public Button buyButton;
        private int cost;


        private void Start()
        {
            PlayerStats.i.onPlayerSetupComplete.AddListener(UpdateButtonInteractable);
        }

        public void SetupUI(UnitData unitData)
        {
            data = unitData;
            cost = UnitManager.GetUnitSo(unitData.id).cost;
            costText.text = cost.ToString();
            unitUI.SetupUI(unitData);
            
            buyButton.onClick.AddListener(OnClickButton);
            PlayerStats.i.onUpdateGold.AddListener(UpdateButtonInteractable);
        }

        private void OnClickButton()
        {
            PlayerStats.AddGold(-cost);
            PlayerStats.AddUnit(data);
        }


        private void UpdateButtonInteractable()
        {
            buyButton.interactable = PlayerStats.GetGold() >= cost;
        }

    }
}