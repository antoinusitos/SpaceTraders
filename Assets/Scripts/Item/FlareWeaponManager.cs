using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class FlareWeaponManager : WeaponManager
    {
        public bool isActive = false;

        protected override void Awake()
        {
        }

        public void Activate()
        {
            GetComponentInChildren<ParticleSystem>().Play();
            GetComponentInChildren<Light>().enabled = true;
            isActive = true;
        }
    }
}