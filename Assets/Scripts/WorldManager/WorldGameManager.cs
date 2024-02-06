using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class WorldGameManager : NetworkBehaviour
    {
        public static WorldGameManager instance = null;

        // Normal players number to start
        public int playersToStart = 4;
        public NetworkVariable<bool> networkGameStarted = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        public NetworkVariable<bool> networkGameFinished = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        private int networkRandomSeed = 0;

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
                Invoke("CheckPlayersNumber", 1);
            }
        }

        private void CheckPlayersNumber()
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

            CheckPlayersNumber();
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
            networkRandomSeed = Random.Range(1, int.MaxValue - 1);
            Random.InitState(networkRandomSeed);

            yield return new WaitForSeconds(0.1f);
            
            PlayerNetworkManager[] playerNetworkManagers = FindObjectsOfType<PlayerNetworkManager>();

            int traitorIndex = Random.Range(0, playerNetworkManagers.Length - 1);

            for (int i = 0; i < playerNetworkManagers.Length; i++)
            {
                playerNetworkManagers[i].HideWaitingPlayerClientRpc();
                if(i == traitorIndex)
                {
                    playerNetworkManagers[i].faction.Value = Factions.TRAITOR;
                }
                else
                {
                    playerNetworkManagers[i].faction.Value = Factions.CREW;
                }
                playerNetworkManagers[i].SetRandomSeedClientRpc(networkRandomSeed);
            }

            yield return new WaitForSeconds(3);

            sasDoor.SetLockState(false);
            sasDoor.ForceOpening();


        }

        [ServerRpc(RequireOwnership = false)]
        public void TestGameFinishedServerRpc()
        {
            Invoke("TestGameFinished", 1);
        }

        private void TestGameFinished()
        {
            PlayerManager[] players = FindObjectsOfType<PlayerManager>();

            for (int i = 0; i < players.Length; i++)
            {
                Debug.Log("player found");
                Debug.Log("health :" + players[i].playerNetworkManager.currentHealth.Value);
                Debug.Log("end game :" + players[i].playerNetworkManager.endGameReached.Value);
                if (players[i].playerNetworkManager.currentHealth.Value > 0 && !players[i].playerNetworkManager.endGameReached.Value)
                {
                    return;
                }
            }

            Debug.Log("game finished");
            networkGameFinished.Value = true;
        }
    }
}