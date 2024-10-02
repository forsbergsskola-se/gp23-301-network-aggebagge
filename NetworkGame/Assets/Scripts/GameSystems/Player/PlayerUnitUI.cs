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
        }
    }
}