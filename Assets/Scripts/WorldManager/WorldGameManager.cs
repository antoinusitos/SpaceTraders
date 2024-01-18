using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class WorldGameManager : NetworkBehaviour
    {
        public static WorldGameManager instance = null;

        public int playersToStart = 4;
        public NetworkVariable<bool> networkGameStarted = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        [SerializeField]
        private AutomaticDoor startingDoor = null;
        [SerializeField]
        private AutomaticDoor sasDoor = null;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;

                //TEMP for 1 player
                Invoke("CheckSoloPlayer", 1);
            }
        }

        private void CheckSoloPlayer()
        {
            if (NetworkManager.Singleton.ConnectedClientsList.Count == playersToStart)
            {
                startingDoor.SetLockState(false);
                startingDoor.ForceOpening();
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
        }

        private void OnClientConnectedCallback(ulong obj)
        {
            if (!IsServer)
            {
                return;
            }

            if (NetworkManager.Singleton.ConnectedClientsList.Count == playersToStart)
            {
                startingDoor.SetLockState(false);
                startingDoor.ForceOpening();
            }
        }

        public void StartGame()
        {
            if(networkGameStarted.Value)
            {
                return;
            }

            networkGameStarted.Value = true;

            StartCoroutine(StartingGame());
        }

        private IEnumerator StartingGame()
        {
            yield return new WaitForSeconds(0.1f);
            
            PlayerNetworkManager[] playerNetworkManagers = FindObjectsOfType<PlayerNetworkManager>();
            for (int i = 0; i < playerNetworkManagers.Length; i++)
            {
                playerNetworkManagers[i].HideWaitingPlayerClientRpc();
                playerNetworkManagers[i].faction.Value = Factions.CREW;
            }

            yield return new WaitForSeconds(3);

            sasDoor.SetLockState(false);
            sasDoor.ForceOpening();
        }
    }
}