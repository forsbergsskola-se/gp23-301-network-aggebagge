using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.Battle
{
    public class BattleCurseUI : MonoBehaviour
    {
        public Color notCurseColor;
        public Image uiImage;
        private bool isCurse;

        public void SetCurse(bool curse)
        {
            isCurse = curse;
            uiImage.color = curse ? Color.white : notCurseColor;
        }

        public bool IsCursed()
        {
            return isCurse;
        }

        public void AddToHUD()
        {
            gameObject.SetActive(true);
            SetCurse(false);
        }
        
        
    }
}