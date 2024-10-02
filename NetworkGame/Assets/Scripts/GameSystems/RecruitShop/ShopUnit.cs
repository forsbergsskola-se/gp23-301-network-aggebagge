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
        private UnitSo unitSo;

        public Button buyButton;
        private int cost;


        private void Start()
        {
            PlayerStats.i.onPlayerSetupComplete.AddListener(UpdateButtonInteractable);
        }

        public void SetupUI(UnitSo unit)
        {
            unitSo = unit;
            costText.text = unit.cost.ToString();
            unitUI.SetupUI(unit);

            cost = unit.cost;
            
            
            
            // if(GameManager.i.hasJoinedRoom)
            //     UpdateButtonInteractable();
            // else
            // {
            //     GameManager.i.onJoinRoom.AddListener(UpdateButtonInteractable);
            //     buyButton.interactable = false;
            // }
            
            buyButton.onClick.AddListener(OnClickButton);
            PlayerStats.i.onUpdateGold.AddListener(UpdateButtonInteractable);
            
        }

        private void OnClickButton()
        {
            PlayerStats.AddGold(-cost);
            PlayerStats.AddUnit(unitSo);
        }


        private void UpdateButtonInteractable()
        {
            buyButton.interactable = PlayerStats.GetGold() >= cost;
        }

    }
}