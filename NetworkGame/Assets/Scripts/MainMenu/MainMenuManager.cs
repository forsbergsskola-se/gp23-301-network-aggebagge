using System;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        public Button createRoomButton;
        public Button joinRoomButton;
        public Button settingsButton;
        public Button quitButton;

        private void Start()
        {
            createRoomButton.onClick.AddListener(CreateRoom);
            joinRoomButton.onClick.AddListener(JoinRoom);
            quitButton.onClick.AddListener(QuitGame);
        }

        private void CreateRoom()
        {
            
        }
        private void JoinRoom()
        {
            
        }
        
        private void QuitGame()
        {
            Application.Quit();
        }
      
    }
}