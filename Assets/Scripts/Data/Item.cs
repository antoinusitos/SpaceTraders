using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class Item : MonoBehaviour
    {
        public CharacterManager itemOwner = null;

        public ItemDefinition itemDefinition = null;

        public virtual void UseItem()
        {

        }
    }
}