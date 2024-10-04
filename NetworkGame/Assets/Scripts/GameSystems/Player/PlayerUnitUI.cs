using System;
using GameSystems.Units;
using TMPro;
using UnityEngine;

namespace GameSystems.Player
{
    public class PlayerUnitUI : UnitUI
    {
        int count = 1;
        public TextMeshProUGUI countText;

        public void AddUnit()
        {
            count++;
            countText.text = count.ToString();
            if (count == 1)
                gameObject.SetActive(false);
        }

        public void RemoveUnit()
        {
            count--;
            countText.text = count.ToString();
            if(count <= 0)
                gameObject.SetActive(false);
            
        }
    }
}