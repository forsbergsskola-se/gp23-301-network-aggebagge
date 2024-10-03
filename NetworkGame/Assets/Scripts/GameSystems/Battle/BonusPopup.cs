using System;
using System.Collections;
using UnityEngine;

namespace GameSystems.Battle
{
    public class BonusPopup : MonoBehaviour
    {
        private float speed = 1;
        private float targetYPos = 1;
        private float startYPos;

        private RectTransform rectTransform;

        private void Awake()
        {
            startYPos = transform.position.y;
            StartCoroutine(DestroyDelay());
            
            rectTransform = GetComponent<RectTransform>();

        }

        private void Update()
        {
            // var position = transform.position;
            // position = new Vector3(position.x, Mathf.Lerp(position.y, startYPos + targetYPos, Time.deltaTime * speed));
            // transform.position = position;
            

            // Get the current anchored position
            Vector2 position = rectTransform.anchoredPosition;

            // Apply the same vertical interpolation using Mathf.Lerp, but to the y component of anchoredPosition
            position = new Vector2(position.x, Mathf.Lerp(position.y, startYPos + targetYPos, Time.deltaTime * speed));

            // Set the updated position back to the RectTransform
            rectTransform.anchoredPosition = position;
        }
        
        IEnumerator DestroyDelay()
        {
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }
    }
}