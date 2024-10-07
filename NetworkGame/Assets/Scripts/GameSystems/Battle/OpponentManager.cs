using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

namespace GameSystems.Battle
{
    public class OpponentManager : MonoBehaviourPunCallbacks
    {
        // public List<OpponentTracker> opponentTrackerList = new();
        [HideInInspector] public Dictionary<int, OpponentTracker> opponentTrackers = new ();

        // Method to prepare opponent trackers for all players
        public void PrepareOpponentTrackers()
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                var tracker = new OpponentTracker(player.ActorNumber);

                // Populate the tracker with the data
                tracker.opponentsMet = GetOpponentsMetForPlayer(player.ActorNumber);
                tracker.currentOpponent = GetCurrentOpponentForPlayer(player.ActorNumber);

                // Update the dictionary directly
                opponentTrackers[player.ActorNumber] = tracker;

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
        private void UpdateOpponentTracker(int playerId, OpponentTracker tracker)
        {
            // Update existing tracker
            // Add new tracker
            opponentTrackers[playerId] = tracker;
        }
        
        
        private List<(int player1, int player2)> FindAllBattles()
        {
            List<(int player1, int player2)> battlePairs = new List<(int player1, int player2)>();
            List<int> availablePlayers = new List<int>();

            // Add all alive players to the availablePlayers list
            foreach (var tracker in opponentTrackers.Values)
            {
                int playerID = tracker.playerId;
                if (GameManager.i.IsPlayerAlive(playerID))
                {
                    availablePlayers.Add(playerID);
                }
                // Reset current opponents from the previous round
                tracker.ResetCurrentOpponent();
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
                    if (!opponentTrackers[player1].HasMet(player2))
                    {
                        // Record the current opponents
                        opponentTrackers[player1].SetCurrentOpponent(player2);
                        opponentTrackers[player2].SetCurrentOpponent(player1);

                        battlePairs.Add((player1, player2));
                        availablePlayers.RemoveAt(j); // Remove player2 since they're now paired
                        break; // Move to the next player1
                    }
                }

                // If there's an odd number of players and no valid opponent found, battle the AI
                if (isOddNumberOfPlayers && !battlePairs.Any(pair => pair.player1 == player1 || pair.player2 == player1))
                {
                    // Player1 battles the AI (player ID -1)
                    opponentTrackers[player1].SetCurrentOpponent(-1); // Set AI as opponent
                    battlePairs.Add((player1, -1));
                    isOddNumberOfPlayers = false; // Reset flag after AI pairing
                }
            }

            return battlePairs;
        }

        // Reset all opponent lists for a new round
        private void ResetAllBattles()
        {
            foreach (var opponentTracker in opponentTrackers.Values)
            {
                opponentTracker.ResetOpponentsMet(); // Clear the list for each player
                opponentTracker.ResetCurrentOpponent(); // Reset the current opponent as well
            }

            Debug.Log("All battles reset! New cycle can begin.");
        }

        // After a battle, mark it as complete
        private void RegisterBattle(int player1, int player2)
        {
            if (opponentTrackers.TryGetValue(player1, out var p1))
            {
               p1.SetCurrentOpponent(player2);
            }

            if (opponentTrackers.TryGetValue(player2, out var p2))
            {
               p2.SetCurrentOpponent(player1);
            }
        }

        public void SetupBattles()
        {
            // After all battles in the round, reset current opponents
            foreach (var opponentTracker in opponentTrackers.Values)
            {
                opponentTracker.ResetCurrentOpponent();
            }

            // Check if all players have met all other players
            List<int> allPlayerIDs = opponentTrackers.Keys.ToList(); // Get all player IDs from the dictionary
            if (opponentTrackers.Values.All(op => op.HasMetAllPlayers(allPlayerIDs)))
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
            int myID = GameManager.i.GetID();

            if (opponentTrackers.TryGetValue(myID, out var opTracker))
                return opTracker.currentOpponent;
    
            Debug.LogWarning("Could not find any opponent?");
            return -1;
        }
    }
}