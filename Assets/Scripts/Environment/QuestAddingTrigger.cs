using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class QuestAddingTrigger : NetworkBehaviour
    {
        [SerializeField]
        private int questID = -1;

        private void OnTriggerEnter(Collider other)
        {
            if (IsServer)
            {
                PlayerManager playerManager = other.GetComponent<PlayerManager>();
                if (playerManager)
                {
                    PlayerNetworkManager[] playerNetworkManagers = FindObjectsOfType<PlayerNetworkManager>();
                    for (int i = 0; i < playerNetworkManagers.Length; i++)
                    {
                        playerNetworkManagers[i].SyncQuestClientRpc(questID);
                    }
                }
            }
        }
    }
}