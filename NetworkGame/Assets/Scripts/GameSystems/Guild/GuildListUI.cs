using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.Guild
{
    public class GuildListUI : MonoBehaviour
    {
        public GuildListObject guildObjectPrefab;
        public Transform layout;
        
        private void Start()
        {
            GuildManager.i.onGuildSynced.AddListener(OnGuildsSynced);
        }

        private void OnGuildsSynced()
        {
            foreach (Transform child in layout)
                Destroy(child.gameObject);
            
            foreach (var guildStats in GuildManager.i.playerGuilds)
            {
                var guildObject = Instantiate(guildObjectPrefab, layout);
                guildObject.SetupUI(guildStats);
            }
        }
    }
}