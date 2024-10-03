using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.Battle
{
    public class BonusPopup : MonoBehaviour
    {
        private float speed = 1;
        private float targetYPos = 1;
        private float startYPos;

        private RectTransform rectTransform;
        public TextMeshProUGUI text;
        public Image image;

        private void Awake()
        {
            startYPos = transform.position.y;
            // StartCoroutine(DestroyDelay());
            
            rectTransform = GetComponent<RectTransform>();

        }

        public void SetText(bool isDamage, int value)
        {
            text.text = value.ToString();
            text.color = isDamage ? GameManager.i.damageColor : GameManager.i.goldColor;
            text.gameObject.SetActive(true);
        }
        
        public void SetIcon(Sprite sprite)
        {
            image.sprite = sprite;
            image.gameObject.SetActive(true);
        }

        // private void Update()
        // {
        //     // var position = transform.position;
        //     // position = new Vector3(position.x, Mathf.Lerp(position.y, startYPos + targetYPos, Time.deltaTime * speed));
        //     // transform.position = position;
        //     
        //
        //     // Get the current anchored position
        //     Vector2 position = rectTransform.anchoredPosition;
        //
        //     // Apply the same vertical interpolation using Mathf.Lerp, but to the y component of anchoredPosition
        //     position = new Vector2(position.x, Mathf.Lerp(position.y, startYPos + targetYPos, Time.deltaTime * speed));
        //
        //     // Set the updated position back to the RectTransform
        //     rectTransform.anchoredPosition = position;
        // }
        
        // IEnumerator DestroyDelay()
        // {
        //     yield return new WaitForSeconds(1.5f);
        //     Destroy(gameObject);
        // }
    }
}