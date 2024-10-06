using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace GameRooms
{
    public class RoomManager  : MonoBehaviourPunCallbacks
    {
        [HideInInspector] public UnityEvent onJoinedRoom = new();
        [HideInInspector] public UnityEvent onUpdatePlayerCount = new();
        
        private string roomCode;
        private int playerCount = 1;

        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            
            if(PlayerPrefsData.i.IsHost())
                CreateRoom();
            else
            {
                roomCode = PlayerPrefsData.i.GetCode();
                JoinRoom(roomCode);
            }
        }

        private void CreateRoom()
        {
             roomCode = GenerateRoomCode();
             
             // Set custom room properties for the room code
             var roomOptions = new RoomOptions();
             Hashtable customRoomProperties = new Hashtable();
             customRoomProperties.Add("roomCode", roomCode);  // Store the code as a custom property
             roomOptions.MaxPlayers = 20;
             roomOptions.CustomRoomProperties = customRoomProperties;
             roomOptions.CustomRoomPropertiesForLobby = new string[] { "roomCode" };  // Make sure this property is visible in the lobby
             PhotonNetwork.CreateRoom(null, roomOptions); // Create a room with default name (or you can use roomCode as the name)
             
        }

        public void JoinRoom(string codeInput)
        {
            // Store the input for future use
            roomCode = codeInput.ToUpper();
    
            // Load the rooms from the lobby
            PhotonNetwork.JoinLobby();
        }

        // Callback when the room list is updated (after joining the lobby)
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach (RoomInfo room in roomList)
            {
                if (room.CustomProperties.ContainsKey("roomCode") && room.CustomProperties["roomCode"].ToString() == roomCode)
                {
                    // Found the room, now join it
                    PhotonNetwork.JoinRoom(room.Name);
                    return;
                }
            }

            // If we reach here, no room was found
            Debug.LogError("Room with code " + roomCode + " not found.");
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            playerCount = PhotonNetwork.PlayerList.Length;
            photonView.RPC("SyncPlayers", RpcTarget.All, playerCount);
            onJoinedRoom.Invoke();
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
            SceneManager.LoadScene("MainMenu");
            Debug.Log("Could not join room");
        }

        private string GenerateRoomCode()
        {
            const string chars = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";
            return new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[Random.Range(0, s.Length)]).ToArray());
        }
        
        [PunRPC]
        void SyncPlayers(int players)
        {
            playerCount = players;
            onUpdatePlayerCount.Invoke();
        }

        public string GetRoomCode()
        {
            return roomCode;
        }
        
    }
}