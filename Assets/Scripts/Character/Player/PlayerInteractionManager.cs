using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class PlayerInteractionManager : CharacterInteractionManager
    {
        [HideInInspector]
        public PlayerManager player = null;

        [SerializeField]
        private LayerMask interactionMask;

        private Interactable lastInteractable = null;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            if(!IsOwner)
            {
                return;
            }

            RaycastHit hit;
            if (Physics.Raycast(player.playerFPSCamera.playerCamera.transform.position, player.playerFPSCamera.playerCamera.transform.forward, out hit, interactionDistance, interactionMask))
            {
                Interactable interactable = hit.transform.GetComponent<Interactable>();
                if (interactable)
                {
                    if (interactable.GetType() == typeof(PickableItem))
                    {
                        PlayerUIManager.instance.playerUIHUDManager.ShowInteractionText(true, "Pick up " + interactable.interactableName);
                    }
                    else
                    {
                        PlayerUIManager.instance.playerUIHUDManager.ShowInteractionText(true, "Use " + interactable.interactableName);
                    }
                }
                else
                {
                    PlayerUIManager.instance.playerUIHUDManager.ShowInteractionText(false);
                }
            }
            else
            {
                PlayerUIManager.instance.playerUIHUDManager.ShowInteractionText(false);
            }
        }

        public override void TryToInteract()
        {
            if (player.isUsingAnInteractable)
            {
                StopInteract();
                return;
            }

            RaycastHit hit;
            if (Physics.Raycast(player.playerFPSCamera.playerCamera.transform.position, player.playerFPSCamera.playerCamera.transform.forward, out hit, interactionDistance, interactionMask))
            {
                Interactable interactable = hit.transform.GetComponent<Interactable>();
                if(interactable)
                {
                    if (interactable.GetType() == typeof(PickableItem))
                    {
                        PickableItem pickableItem = (PickableItem) interactable;
                        if(pickableItem)
                        {
                            pickableItem.OnInteract(this);
                        }
                    }
                    else
                    {
                        if (interactable.OnInteract(this))
                        {
                            lastInteractable = interactable;
                            player.isUsingAnInteractable = true;
                        }
                    }
                }
            }
        }

        public void StopInteract()
        {
            player.isUsingAnInteractable = false;
            if (lastInteractable)
            {
                lastInteractable.OnStopInteract();
                lastInteractable = null;
            }
        }

#region MACHINES
        [ServerRpc]
        private void TellServerUsingStateServerRpc(bool newState, ulong ID)
        {
            SetUsingStateOnMachine(newState, ID);
        }

        public void SetUsingStateOnMachine(bool newState, ulong ID)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                SetUsingStateOnMachine_Implementation(newState, ID);
            }
            else
            {
                TellServerUsingStateServerRpc(newState, ID);
            }
        }

        private void SetUsingStateOnMachine_Implementation(bool newState, ulong ID)
        {
            NetworkObject foundObject = null;
            NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(ID, out foundObject);
            if (foundObject)
            {
                Machine machine = foundObject.GetComponent<Machine>();
                if (machine != null)
                {
                    machine.machineIsBeingUSed.Value = newState;
                }
            }
        }

        [ServerRpc]
        private void TellServerIsDoneServerRpc(ulong ID)
        {
            SetIsDoneOnMachine(ID);
        }

        public void SetIsDoneOnMachine(ulong ID)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                SetIsDoneOnMachine_Implementation(ID);
            }
            else
            {
                TellServerIsDoneServerRpc(ID);
            }
        }

        private void SetIsDoneOnMachine_Implementation(ulong ID)
        {
            NetworkObject foundObject = null;
            NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(ID, out foundObject);
            if (foundObject)
            {
                Machine machine = foundObject.GetComponent<Machine>();
                if (machine != null)
                {
                    machine.isDone.Value = true;
                    machine.OnComplete();
                }
            }
        }

        #endregion
    }
}