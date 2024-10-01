using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.Guild
{
    public class GuildListUI : MonoBehaviour
    {
        public GuildListObject guildObjectPrefab;
        public Transform layout;

        private List<GuildListObject> guildObjects = new ();
        
        
        private void Start()
        {
            foreach (var guildStats in GameManager.i.playerGuilds)
            {
                var guildObject = Instantiate(guildObjectPrefab, layout);
                guildObject.SetupUI(guildStats);
                guildObjects.Add(guildObject);
            }
        }

        private void UpdateUI(int playerId, int hp)
        {
            guildObjects[playerId].UpdateUI(hp.ToString());
        }
    }
}