using System;
using GameSystems.Units;
using TMPro;
using UnityEngine;

namespace Tooltip
{
    public class TooltipUI : MonoBehaviour
    {
        private static TooltipUI i;
        
        public TextMeshProUGUI characterText;
        public TextMeshProUGUI text;

        private RectTransform rectTransform;
        public Canvas canvas;

        public Vector2 offset;
        
        private void Awake()
        {
            i = this;
            rectTransform = GetComponent<RectTransform>();
            gameObject.SetActive(false);
        }

        private void Update()
        {
            // Get the mouse position in screen space
            Vector2 mousePosition = Input.mousePosition;

            // Convert the screen position to canvas position
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, mousePosition, canvas.worldCamera, out Vector2 canvasPosition);

            // Set the position of the UI object to the mouse position (within the canvas)
            rectTransform.anchoredPosition = canvasPosition + offset;
        }

        private void DisplayTooltip(UnitSo unitSo, UnitData unitData)
        {
            gameObject.SetActive(true);
            characterText.text = unitSo.unitName;

            string tooltipText = unitSo.tooltipText;
            tooltipText = tooltipText.Replace("dmg", unitData.damage.ToString());
            tooltipText = tooltipText.Replace("gg", unitData.goldGain.ToString());

            text.text = tooltipText;
        }
   
        
        
        public static void DisplayTooltip(UnitData unitData)
        {
            i.DisplayTooltip(UnitManager.GetUnitSo(unitData.id), unitData);
        }
        public static void HideTooltip()
        {
            i.gameObject.SetActive(false);
        }
    }
}