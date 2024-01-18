using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class HealConsummable : Consumable
    {
        [SerializeField]
        private int healAmount = 25;

        public override void UseItem()
        {
            base.UseItem();

            if(itemOwner)
            {
                itemOwner.characterNetworkManager.currentHealth.Value += healAmount;
            }
        }
    }
}