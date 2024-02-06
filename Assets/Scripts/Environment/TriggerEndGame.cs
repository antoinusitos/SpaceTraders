using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class TriggerEndGame : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            PlayerManager playerManager = other.GetComponent<PlayerManager>();
            if (playerManager && playerManager.IsOwner)
            {
                playerManager.playerNetworkManager.endGameReached.Value = true;
                WorldGameManager.instance.TestGameFinishedServerRpc();
            }
        }
    }
}