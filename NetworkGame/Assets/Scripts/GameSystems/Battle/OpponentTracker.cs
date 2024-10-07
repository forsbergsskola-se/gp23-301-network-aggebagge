using ExitGames.Client.Photon;
using System.Collections.Generic;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace GameSystems.Battle
{
    public class OpponentTracker
    {
        public int playerId;
        public List<int> opponentsMet;
        public int currentOpponent; // Store the current opponent's ID

        public OpponentTracker(int id)
        {
            playerId = id;
            opponentsMet = new List<int>();
            currentOpponent = -1; // Initialize as -1 (no current opponent)
        }
        

        // Check if this player has met a given opponent
        public bool HasMet(int opponentID)
        {
            return opponentsMet.Contains(opponentID);
        }

        // Track the current opponent
        public void SetCurrentOpponent(int opponentID)
        {
            currentOpponent = opponentID;
            if (opponentID != -1) // Don't log AI (-1) as an opponent
            {
                opponentsMet.Add(opponentID);
            }
        }

        // Reset the current opponent (after battles)
        public void ResetCurrentOpponent()
        {
            currentOpponent = -1;
        }

        // Reset the entire list of opponents met (for a new cycle)
        public void ResetOpponentsMet()
        {
            opponentsMet.Clear(); // Clear the opponents list
        }

        // Check if the player has met all other players
        public bool HasMetAllPlayers(List<int> allPlayerIDs)
        {
            return allPlayerIDs.All(id => opponentsMet.Contains(id) || id == playerId);
        }
        
        
        // Convert to Hashtable for easier serialization
        public Hashtable ToHashtable()
        {
            Hashtable hashtable = new Hashtable
            {
                { "PlayerId", playerId },
                { "CurrentOpponent", currentOpponent },
                { "OpponentsMet", opponentsMet.ToArray() } // Convert list to array for serialization
            };
        
            return hashtable;
        }

        // Create from Hashtable
        public static OpponentTracker FromHashtable(Hashtable hashtable)
        {
            int id = (int)hashtable["PlayerId"];
            int currentOpponent = (int)hashtable["CurrentOpponent"];
            int[] opponentsMetArray = (int[])hashtable["OpponentsMet"];
        
            OpponentTracker tracker = new OpponentTracker(id)
            {
                currentOpponent = currentOpponent,
                opponentsMet = new List<int>(opponentsMetArray)
            };

            return tracker;
        }
        
    }
}