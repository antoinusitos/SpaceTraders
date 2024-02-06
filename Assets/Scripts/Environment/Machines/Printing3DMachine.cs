using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class Printing3DMachine : Machine
    {
        public NetworkVariable<float> printCompletion = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        public NetworkVariable<bool> isPrinting = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        public NetworkVariable<bool> itemIsReady = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        public NetworkVariable<int> currentItemID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        //Server side
        private ItemDefinition craftingItem = null;

        [SerializeField]
        private Slider progressionSlider = null;

        private void Awake()
        {
            printCompletion.OnValueChanged += OnPrintCompletionChange;
        }

        private void OnPrintCompletionChange(float previousValue, float newValue)
        {
            progressionSlider.value = newValue;
        }

        public override bool OnInteract(PlayerInteractionManager playerUsing)
        {
            if (itemIsReady.Value)
            {
                if (playerUsing.player.characterInventoryManager.AddItem(WorldItemsManager.instance.GetItemWithID(currentItemID.Value)))
                {
                    NotifyServerItemIsTakenServerRpc();
                }
                return false;
            }

            if(isPrinting.Value)
            {
                return false;
            }

            if (!base.OnInteract(playerUsing))
            {
                return false;
            }

            player = playerUsing;

            return true;
        }

        [ServerRpc(RequireOwnership = false)]
        public void NotifyServerItemIsTakenServerRpc()
        {
            itemIsReady.Value = false;
        }

        public override void OnStopInteract()
        {
            base.OnStopInteract();

            player = null;
        }

        public void PrintItem(int itemID)
        {
            if(isPrinting.Value || itemIsReady.Value)
            {
                return;
            }

            ItemDefinition itemDef = WorldItemsManager.instance.GetItemWithID(itemID);
            if (itemDef.craftPrice > player.player.playerNetworkManager.cash.Value)
            {
                return;
            }

            player.player.playerNetworkManager.cash.Value -= itemDef.craftPrice;

            if (player)
            {
                player.StopInteract();
            }

            PrintItemServerRpc(itemID);
        }

        [ServerRpc(RequireOwnership = false)]
        public void PrintItemServerRpc(int itemID)
        {
            currentItemID.Value = itemID;
            craftingItem = WorldItemsManager.instance.GetItemWithID(itemID);
            isPrinting.Value = true;
        }

        private void Update()
        {
            if(!IsServer)
            {
                return;
            }

            if(isPrinting.Value)
            {
                printCompletion.Value += Time.deltaTime / craftingItem.craftTime;
                if(printCompletion.Value > 1)
                {
                    printCompletion.Value = 0;
                    isPrinting.Value = false;
                    itemIsReady.Value = true;
                }
            }
        }
    }
}