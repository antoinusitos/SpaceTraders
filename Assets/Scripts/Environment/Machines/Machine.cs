using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace AG
{
    public class Machine : Interactable
    {
        public NetworkVariable<bool> machineIsBeingUSed = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        public NetworkVariable<bool> isDone = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        [SerializeField]
        protected GameObject machineUI = null;

        protected PlayerInteractionManager player = null;

        [SerializeField]
        protected UnityEvent onCompleteEvents = null;

        [SerializeField]
        protected int cashEarnedWhenComplete = 20;

        public override bool OnInteract(PlayerInteractionManager playerUsing)
        {
            if (machineIsBeingUSed.Value)
            {
                return false;
            }

            if (isDone.Value)
            {
                return false;
            }

            playerUsing.player.currentFocusType = FocusType.UI;
            playerUsing.SetUsingStateOnMachine(true, NetworkObject.NetworkObjectId);
            machineUI.SetActive(true);

            return true;
        }

        public override void OnStopInteract()
        {
            if(player)
            {
                player.player.currentFocusType = FocusType.Game;
                player.SetUsingStateOnMachine(false, NetworkObject.NetworkObjectId);
            }
            machineUI.SetActive(false);
        }

        //Server Side
        public void OnComplete()
        {
            onCompleteEvents.Invoke();
        }
    }
}