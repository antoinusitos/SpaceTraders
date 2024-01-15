using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class ForceAutomaticDoor : NetworkBehaviour
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
                automaticDoor.ForceOpening();
                automaticDoor.SetLockState(false);
            }
        }
    }
}