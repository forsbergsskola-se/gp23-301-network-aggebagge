using System;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
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
            CreateRoom();
        }

        private void CreateRoom()
        {
             roomCode = GenerateRoomCode();
             
             // Set custom room properties for the room code
             var roomOptions = new RoomOptions();
             Hashtable customRoomProperties = new Hashtable();
             customRoomProperties.Add("roomCode", roomCode);  // Store the code as a custom property
             roomOptions.CustomRoomProperties = customRoomProperties;

             PhotonNetwork.CreateRoom(null, roomOptions); // Create a room with default name (or you can use roomCode as the name)
        }

        public void JoinRoom(string input)
        {
            PhotonNetwork.JoinRoom(roomCode);
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