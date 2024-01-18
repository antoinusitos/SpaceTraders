using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class Item : NetworkBehaviour
    {
        public CharacterManager itemOwner = null;

        public int linkedItemId = -1;

        [HideInInspector]
        public ItemDefinition itemDefinition = null;

        public virtual void UseItem()
        {

        }
    }
}