using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class EventTransmitter : MonoBehaviour
    {
        private PlayerEquipmentManager player = null;

        private void Awake()
        {
            player = GetComponentInParent<PlayerEquipmentManager>();
        }

        public void OpenDamageCollider()
        {
            player.OpenDamageCollider();
        }

        public void CloseDamageCollider()
        {
            player.CloseDamageCollider();
        }
    }
}