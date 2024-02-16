using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class WorldGameSessionManager : MonoBehaviour
    {
        public static WorldGameSessionManager instance = null;

        [Header("Active players in Session")]
        public List<PlayerManager> players = new List<PlayerManager>();

        [SerializeField]
        private Color[] colors;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AddPlayerToActivePlayersList(PlayerManager player)
        {
            if(!players.Contains(player))
            {
                players.Add(player);
            }

            for(int i = players.Count - 1; i >= 0; i--)
            {
                if (players[i] == null)
                {
                    players.RemoveAt(i);
                }
            }

            if (NetworkManager.Singleton.IsServer && WorldGameManager.instance)
            {
                if (players.Count == WorldGameManager.instance.playersToStart)
                {
                    Invoke("AffectColors", 1);
                }
            }
        }

        private void AffectColors()
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].playerNetworkManager.playerColor.Value = colors[i];
            }
        }

        public void RemovePlayerFromActivePlayersList(PlayerManager player)
        {
            players.Remove(player);

            for (int i = players.Count - 1; i >= 0; i--)
            {
                if (players[i] == null)
                {
                    players.RemoveAt(i);
                }
            }
        }
    }
}