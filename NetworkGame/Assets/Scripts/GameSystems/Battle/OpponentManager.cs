using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.Battle
{
    public class OpponentManager
    {
        public List<OpponentsMet> allOpponentsMet = new();
        public List<int> allPlayers = new(); // This should be filled with player IDs

        public OpponentManager(List<int> players)
        {
            allPlayers = players;
            // Initialize OpponentsMet for each player
            foreach (var player in allPlayers)
            {
                allOpponentsMet.Add(new OpponentsMet(player));
            }
        }

        public (int player1, int player2) FindNextBattle()
        {
            bool isOddNumberOfPlayers = allOpponentsMet.Count % 2 != 0;

            // Iterate through all players
            for (int i = 0; i < allOpponentsMet.Count; i++)
            {
                int player1 = allOpponentsMet[i].playerID;

                // Check if player1 is still alive
                if (!GameManager.i.IsPlayerAlive(player1))
                    continue; // Skip this player if they are dead

                for (int j = i + 1; j < allOpponentsMet.Count; j++)
                {
                    int player2 = allOpponentsMet[j].playerID;

                    // Check if both players are alive
                    if (GameManager.i.IsPlayerAlive(player2))
                    {
                        // Check if player1 and player2 haven't battled yet
                        if (!allOpponentsMet[i].HasMet(player2))
                        {
                            return (player1, player2); // Return the next available pair
                        }
                    }
                }

                // If there's an odd number of players, have the remaining player battle the AI
                if (isOddNumberOfPlayers && !allOpponentsMet[i].HasMet(-1))
                {
                    // Player1 battles the AI (player ID -1)
                    return (player1, -1);
                }
            }

            // No valid pairs available
            return (-1, -1); // All possible pairs have met or players are dead
        }

        // Reset all opponent lists for a new round
        public void ResetAllBattles()
        {
            foreach (var opponentsMet in allOpponentsMet)
            {
                opponentsMet.ResetOpponents();
            }
        }

        // After a battle, mark it as complete
        public void RegisterBattle(int player1, int player2)
        {
            var p1 = allOpponentsMet.Find(p => p.playerID == player1);
            var p2 = allOpponentsMet.Find(p => p.playerID == player2);

            p1?.AddOpponent(player2);
            p2?.AddOpponent(player1);
        }
    }
}