using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        public Button createRoomButton;
        public Button joinRoomButton;
        public Button settingsButton;
        public Button quitButton;

        public Button joinWithCodeButton;
        public Button returnButton;

        public Transform joinWidgetTransform;
        public TMP_InputField roomcodeInputField;

        public Transform mainMenuTransform;
        
        private void Start()
        {
            createRoomButton.onClick.AddListener(CreateRoomClick);
            joinRoomButton.onClick.AddListener(JoinRoomClick);
            quitButton.onClick.AddListener(QuitGame);
            joinWithCodeButton.onClick.AddListener(JoinWithCodeClick);
            returnButton.onClick.AddListener(ReturnToMainMenu);
            
        }

        private void ReturnToMainMenu()
        {
            joinWidgetTransform.gameObject.SetActive(false);
            mainMenuTransform.gameObject.SetActive(true);
        }

        private void JoinWithCodeClick()
        {
            PlayerPrefsData.i.SavePrefsData(false, roomcodeInputField.text.ToUpper());
            SceneManager.LoadScene("GameScene");
        }

        private void CreateRoomClick()
        {
            PlayerPrefsData.i.SavePrefsData(true, "");
            SceneManager.LoadScene("GameScene");

        }
        private void JoinRoomClick()
        {
            joinWidgetTransform.gameObject.SetActive(true);
            mainMenuTransform.gameObject.SetActive(false);
        }
        
        
        private void QuitGame()
        {
            Application.Quit();
        }
      
    }
}