using System.Collections.Generic;

namespace GameSystems.Battle
{
    public class OpponentsMet
    {
        public int playerID;
        private List<int> opponents;

        public OpponentsMet(int id)
        {
            playerID = id;
            opponents = new List<int>();
        }
            
        public void AddOpponent(int opponentID)
        {
            if (!opponents.Contains(opponentID))
            {
                opponents.Add(opponentID);
            }
        }

        public bool HasMet(int opponentID)
        {
            return opponents.Contains(opponentID);
        }

        public void ResetOpponents()
        {
            opponents.Clear();
        }
    }
}