using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;

namespace GameSystems.Guild
{
    [System.Serializable]
    public class GuildStats
    {
        public int playerID;
        public string guildName;
        // public int maxHp;
        public int hp;
        // public int gold;
        public int groupSize;
        public Color guildColor;

        public GuildStats(int id, string gName, Color color, int gSize, int gHp)
        {
            playerID = id;
            guildName = gName;
            guildColor = color;
            groupSize = gSize;
            hp = gHp;
        }

        public GuildStats()
        {}
        
        
        // Convert to Hashtable for easier serialization
        public Hashtable ToHashtable()
        {
            Hashtable hashtable = new Hashtable
            {
                { "Id", playerID },
                { "Name", guildName },
                { "Color", ColorToString(guildColor) }, // Use a method to convert color to string
                { "GroupSize", groupSize },
                { "Hp", hp }

                
            };
            
            return hashtable;
        }

        // Create from Hashtable
        public static GuildStats FromHashtable(Hashtable hashtable)
        {
            int id = (int)hashtable["Id"];
            string name = (string)hashtable["Name"];
            Color color = StringToColor((string)hashtable["Color"]); // Convert string back to color
            int groupSize = (int)hashtable["GroupSize"];
            int hp = (int)hashtable["Hp"];

            return new GuildStats(id, name, color, groupSize, hp);
        }
        
        private static string ColorToString(Color color)
        {
            return ColorUtility.ToHtmlStringRGB(color); // Convert color to a string representation
        }

        private static Color StringToColor(string colorString)
        {
            ColorUtility.TryParseHtmlString("#" + colorString, out var color); // Convert string back to color
            return color;
        }
        
    }
}