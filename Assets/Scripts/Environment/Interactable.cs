using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class Interactable : NetworkBehaviour
    {
        public string interactableName = "UNKNOWN";

        public virtual bool OnInteract(PlayerInteractionManager playerUsing)
        {
            return false;
        }

        public virtual void OnStopInteract()
        {

        }
    }
}