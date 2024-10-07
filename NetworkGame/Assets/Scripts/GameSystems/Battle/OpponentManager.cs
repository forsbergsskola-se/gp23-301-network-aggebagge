using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

namespace GameSystems.Battle
{
    public class OpponentManager : MonoBehaviourPunCallbacks
    {
        public List<OpponentTracker> opponentTrackerList = new();
        private Dictionary<int, OpponentTracker> opponentTrackers = new ();

        // Method to prepare opponent trackers for all players
        public void PrepareOpponentTrackers()
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                var tracker = new OpponentTracker(player.ActorNumber); // Pass the playerId here

                // Populate the tracker with the data
                tracker.opponentsMet = GetOpponentsMetForPlayer(player.ActorNumber);
                tracker.currentOpponent = GetCurrentOpponentForPlayer(player.ActorNumber);

                // Send the data to all players
                Hashtable serializedData = tracker.ToHashtable();
                photonView.RPC("ReceiveOpponentTracker", RpcTarget.All, player.ActorNumber, serializedData);
            }
        }

        // Method to get the list of opponents met by a player
        private List<int> GetOpponentsMetForPlayer(int playerId)
        {
            // Return the list of opponents met for the player
            // This is just a placeholder. Implement logic as per your game.
            if (opponentTrackers.TryGetValue(playerId, out OpponentTracker tracker))
            {
                return tracker.opponentsMet;
            }

            // Return an empty list if no opponents have been met
            return new List<int>();
        }

        // Method to get the current opponent for a player
        private int GetCurrentOpponentForPlayer(int playerId)
        {
            // Return the current opponent for the player
            // This is just a placeholder. Implement logic as per your game.
            if (opponentTrackers.TryGetValue(playerId, out OpponentTracker tracker))
            {
                return tracker.currentOpponent;
            }

            // Return -1 if no current opponent exists
            return -1; // Indicates no current opponent
        }

        // RPC method to receive the data on each client
        [PunRPC]
        public void ReceiveOpponentTracker(int playerId, Hashtable data)
        {
            OpponentTracker tracker = OpponentTracker.FromHashtable(data);
            UpdateOpponentTracker(playerId, tracker); // Update or add the tracker
        }

        // Method to update the opponent tracker
        public void UpdateOpponentTracker(int playerId, OpponentTracker tracker)
        {
            if (opponentTrackers.ContainsKey(playerId))
            {
                opponentTrackers[playerId] = tracker; // Update existing tracker
            }
            else
            {
                opponentTrackers.Add(playerId, tracker); // Add new tracker
            }
        }
        
        
        private List<(int player1, int player2)> FindAllBattles()
        {
            List<(int player1, int player2)> battlePairs = new List<(int player1, int player2)>();
            List<int> availablePlayers = new List<int>();

            // Add all alive players to the availablePlayers list
            foreach (var opponentTracker in opponentTrackerList)
            {
                int playerID = opponentTracker.playerId;
                if (GameManager.i.IsPlayerAlive(playerID))
                {
                    availablePlayers.Add(playerID);
                }
                // Reset current opponents from the previous round
                opponentTracker.ResetCurrentOpponent();
            }

            bool isOddNumberOfPlayers = availablePlayers.Count % 2 != 0;

            // Match players with each other
            for (int i = 0; i < availablePlayers.Count; i++)
            {
                int player1 = availablePlayers[i];

                // Try to find an opponent for player1
                for (int j = i + 1; j < availablePlayers.Count; j++)
                {
                    int player2 = availablePlayers[j];

                    // Check if player1 and player2 haven't battled yet
                    if (!opponentTrackerList[i].HasMet(player2))
                    {
                        // Record the current opponents
                        opponentTrackerList[i].SetCurrentOpponent(player2);
                        opponentTrackerList[j].SetCurrentOpponent(player1);

                        battlePairs.Add((player1, player2));
                        availablePlayers.RemoveAt(j); // Remove player2 since they're now paired
                        break; // Move to the next player1
                    }
                }

                // If there's an odd number of players and no valid opponent found, battle the AI
                if (isOddNumberOfPlayers && !battlePairs.Any(pair => pair.player1 == player1 || pair.player2 == player1))
                {
                    // Player1 battles the AI (player ID -1)
                    opponentTrackerList[i].SetCurrentOpponent(-1); // Set AI as opponent
                    battlePairs.Add((player1, -1));
                    isOddNumberOfPlayers = false; // Reset flag after AI pairing
                }
            }

            return battlePairs;
        }

        // Reset all opponent lists for a new round
        private void ResetAllBattles()
        {
            foreach (var opponentMet in opponentTrackerList)
            {
                opponentMet.ResetOpponentsMet(); // Clear the list for each player
                opponentMet.ResetCurrentOpponent(); // Reset the current opponent as well
            }

            Debug.Log("All battles reset! New cycle can begin.");
        }

        // After a battle, mark it as complete
        private void RegisterBattle(int player1, int player2)
        {
            var p1 = opponentTrackerList.Find(p => p.playerId == player1);
            var p2 = opponentTrackerList.Find(p => p.playerId == player2);

            p1?.SetCurrentOpponent(player2);
            p2?.SetCurrentOpponent(player1);
        }

        public void SetupBattles()
        {
            // After all battles in the round, reset current opponents
            foreach (var opponentMet in opponentTrackerList)
            {
                opponentMet.ResetCurrentOpponent();
            }

            // Check if all players have met all other players
            List<int> allPlayerIDs = opponentTrackerList.Select(op => op.playerId).ToList();
            if (opponentTrackerList.All(op => op.HasMetAllPlayers(allPlayerIDs)))
            {
                // Reset the opponentsMet list when all players have battled everyone
                ResetAllBattles();
            }
            
            List<(int player1, int player2)> allBattlePairs = FindAllBattles();

            foreach (var pair in allBattlePairs)
            {
                if (pair.player2 == -1)
                {
                    Debug.Log($"Player {pair.player1} is battling the AI!");
                }
                else
                {
                    Debug.Log($"Player {pair.player1} vs Player {pair.player2}");
                }

                // Perform the battle between player1 and player2 (or AI)
                RegisterBattle(pair.player1, pair.player2);
            }
        }

        public int GetMyOpponentID()
        {
            var opTracker = opponentTrackerList.FirstOrDefault(opt => opt.playerId == GameManager.i.GetID());

            if (opTracker == null)
            {
                Debug.LogWarning("Could not find any opponent?");
                return -1;
            }
            
            return opTracker.currentOpponent;
        }
    }
}