using System.Collections.Generic;
using GameSystems.Units;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tooltip
{
    public class TooltipHoverManager : MonoBehaviour
    {
        private GraphicRaycaster graphicRaycaster;
        private PointerEventData pointerEventData;
        
        private void Start()
        {
            // Get the GraphicRaycaster component from the Canvas
            graphicRaycaster = GetComponent<GraphicRaycaster>();
        }
        void Update()
        {
            // Create a new PointerEventData
            pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition // Set the position to the current mouse position
            };

            // Create a list to store the results of the raycast
            var results = new List<RaycastResult>();
            graphicRaycaster.Raycast(pointerEventData, results);

            // Check if any of the results contain the UnitUI component
            foreach (var result in results)
            {
                var unitUI = result.gameObject.GetComponent<UnitUI>();
                if (unitUI != null)
                {
                    TooltipUI.DisplayTooltip(unitUI.data);
                    return;
                }
            }

            // If we get here, the mouse is not over any UnitUI element
            TooltipUI.HideTooltip();
        }
        
    }
}