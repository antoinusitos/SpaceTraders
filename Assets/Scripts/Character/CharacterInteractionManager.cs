using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class CharacterInteractionManager : NetworkBehaviour
    {
        [SerializeField]
        protected float interactionDistance = 2.0f;

        public virtual void TryToInteract()
        {

        }
    }
}