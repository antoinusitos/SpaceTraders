using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class ForceAutomaticDoor : NetworkBehaviour
    {
        [SerializeField]
        private AutomaticDoor automaticDoor = null;

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