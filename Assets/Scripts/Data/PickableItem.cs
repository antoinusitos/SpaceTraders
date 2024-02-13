using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class PickableItem : Interactable
    {
        public override bool OnInteract(PlayerInteractionManager playerUsing)
        {
            if (transform.GetChild(0).GetComponent<Item>())
            {
                Item item = transform.GetChild(0).GetComponent<Item>();

                if (playerUsing.player.characterInventoryManager.AddItem(item))
                {
                    playerUsing.player.characterNetworkManager.DespawnItem(NetworkObject.NetworkObjectId);
                }
                return true;
            }

            return false;
        }
    }
}