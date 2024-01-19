using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class PlayerInteractionManager : CharacterInteractionManager
    {
        private PlayerManager player = null;

        [SerializeField]
        private LayerMask interactionMask;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        public override void TryToInteract()
        {
            RaycastHit hit;
            if (Physics.Raycast(player.playerFPSCamera.playerCamera.transform.position, player.playerFPSCamera.playerCamera.transform.forward, out hit, interactionDistance, interactionMask))
            {
                Interactable interactable = hit.transform.GetComponent<Interactable>();
                if(interactable)
                {
                    if(interactable.GetType().IsSubclassOf(typeof(Item)) || interactable.GetType() == typeof(Item))
                    {
                        Item item = (Item)interactable;

                        if (player.characterInventoryManager.AddItem(item))
                        {
                            player.characterNetworkManager.DespawnItem(item.NetworkObject.NetworkObjectId);
                        }
                    }
                }
            }
        }
    }
}