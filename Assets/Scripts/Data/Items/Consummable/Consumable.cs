using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class Consumable : Item
    {
        public override void UseItem()
        {
            Debug.Log("Using Consumable");
        }
    }
}