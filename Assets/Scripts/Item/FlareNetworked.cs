using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class FlareNetworked : NetworkBehaviour
    {
        private float strengh = 10;

        public void Throw(Vector3 dir)
        {
            transform.parent = null;
            GetComponentInChildren<Collider>().isTrigger = false;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = (dir + Vector3.up).normalized * strengh;
        }
    }
}