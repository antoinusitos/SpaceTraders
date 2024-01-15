using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class LockDoor : NetworkBehaviour
    {
        [SerializeField]
        private AutomaticDoor automaticDoor = null;

        private void Start()
        {
            GetComponent<NetworkObject>().Spawn();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsServer)
            {
                return;
            }

            if (other.GetComponent<PlayerManager>())
            {
                automaticDoor.SetLockState(true);
            }
        }
    }
}