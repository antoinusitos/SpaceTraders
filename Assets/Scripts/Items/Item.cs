using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class Item : Interactable
    {
        public CharacterManager itemOwner = null;

        public int linkedItemId = -1;

        //[HideInInspector]
        public ItemDefinition itemDefinition = null;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            itemDefinition = WorldItemsManager.instance.GetItemWithID(linkedItemId);
        }

        public virtual void UseItem()
        {

        }

        [ServerRpc]
        public void DespawnItemServerRpc()
        {
            GetComponent<NetworkObject>().Despawn(true);
        }
    }
}