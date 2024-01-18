using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class SasTrigger : NetworkBehaviour
    {
        private int playersInTrigger = 0;

        [SerializeField]
        private AutomaticDoor doorToLock = null;

        private void OnTriggerEnter(Collider other)
        {
            if (WorldGameManager.instance.networkGameStarted.Value)
            {
                return;
            }

            PlayerManager playerManager = other.GetComponent<PlayerManager>();
            if (playerManager != null)
            {
                if (IsServer)
                {
                    playersInTrigger++;

                    if (playersInTrigger >= WorldGameManager.instance.playersToStart)
                    {
                        doorToLock.ForceClosing();
                        doorToLock.SetLockState(true);
                        WorldGameManager.instance.StartGame();
                    }
                }
                if(playerManager.IsOwner)
                {
                    PlayerUIManager.instance.playerUIHUDManager.ShowWaitingPlayers(true);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (WorldGameManager.instance.networkGameStarted.Value)
            {
                return;
            }

            PlayerManager playerManager = other.GetComponent<PlayerManager>();
            if (playerManager != null)
            {
                if (IsServer)
                {
                    playersInTrigger--;
                }
                else if (playerManager.IsOwner)
                {
                    PlayerUIManager.instance.playerUIHUDManager.ShowWaitingPlayers(false);
                }
            }
        }
    }
}