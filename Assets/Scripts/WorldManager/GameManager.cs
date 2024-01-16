using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

namespace AG
{
    public class GameManager : NetworkBehaviour
    {
        public static GameManager instance = null;

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

        private void Start()
        {
            if (IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;

                //TEMP
                if (NetworkManager.Singleton.ConnectedClientsList.Count == playersToStart)
                {
                    startingDoor.SetLockState(false);
                    startingDoor.ForceOpening();
                }
            }
        }

        private void OnClientConnectedCallback(ulong obj)
        {
            if(NetworkManager.Singleton.ConnectedClientsList.Count + 1 == playersToStart)
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
            }

            yield return new WaitForSeconds(3);

            sasDoor.SetLockState(false);
            sasDoor.ForceOpening();
        }
    }
}