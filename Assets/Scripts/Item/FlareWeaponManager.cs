using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class FlareWeaponManager : WeaponManager
    {
        public bool isActive = false;

        private bool isLaunched = false;
        private float strengh = 10;
        private Rigidbody addedRigidBody = null;

        protected override void Awake()
        {
        }

        public void Activate()
        {
            GetComponentInChildren<ParticleSystem>().Play();
            GetComponentInChildren<Light>().enabled = true;
            isActive = true;
        }

        public void Throw(Vector3 dir)
        {
            transform.parent = null;
            GetComponentInChildren<Collider>().isTrigger = false;
            isLaunched = true;
            addedRigidBody = gameObject.AddComponent<Rigidbody>();
            addedRigidBody.velocity = dir * strengh;
        }

        private void Update()
        {
            if(!isLaunched)
            {
                return;
            }


        }
    }
}