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
            GameManager.i.onStartGame.AddListener(OnJoinRoom);
        }

        private void OnJoinRoom()
        {
            foreach (var guildStats in GuildManager.i.playerGuilds)
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