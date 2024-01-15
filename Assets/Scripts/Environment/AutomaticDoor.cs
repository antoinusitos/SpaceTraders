using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class AutomaticDoor : NetworkBehaviour
    {
        public bool startLocked = false;

        public NetworkVariable<bool> networkCharacterNearby = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        public NetworkVariable<bool> networkLocked = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        private void Start()
        {
            networkCharacterNearby.OnValueChanged += OnValueChanged;
            GetComponent<NetworkObject>().Spawn();

            if(IsServer)
            {
                networkLocked.Value = startLocked;
            }
        }

        private void OnValueChanged(bool previousValue, bool newValue)
        {
            GetComponent<Animator>().SetBool("character_nearby", newValue);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsServer)
            {
                return;
            }

            if(networkLocked.Value)
            {
                return;
            }

            if (other.GetComponent<PlayerManager>())
            {
                networkCharacterNearby.Value = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!IsServer)
            {
                return;
            }

            if (other.GetComponent<CharacterManager>())
            {
                networkCharacterNearby.Value = false;
            }
        }

        public void ForceOpening()
        {
            networkCharacterNearby.Value = true;
        }

        public void SetLockState(bool locked)
        {
            networkLocked.Value = locked;
        }
    }
}