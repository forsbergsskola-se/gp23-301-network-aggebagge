using System;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace GameRooms
{
    public class RoomManager  : MonoBehaviourPunCallbacks
    {
        public UnityEvent onUpdatePlayerCount = new();
        
        private string roomCode;
        private int playerCount = 1;

        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public void CreateRoom()
        {
             roomCode = GenerateRoomCode();
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
        
    }
}