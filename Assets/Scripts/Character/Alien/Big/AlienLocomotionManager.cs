using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace AG
{
    public class AlienLocomotionManager : CharacterLocomotionManager
    {
        protected PlayerManager targetPlayer = null;

        protected AlienManager alienManager = null;
        
        protected NavMeshAgent navMeshAgent = null;

        protected override void Awake()
        {
            base.Awake();

            alienManager = GetComponent<AlienManager>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        protected override void Update()
        {
            base.Update();

            //server check
            if (!alienManager.IsOwner)
            {
                return;
            }

            if(!targetPlayer)
            {
                targetPlayer= FindObjectOfType<PlayerManager>();
            }
            else
            {
                navMeshAgent.SetDestination(targetPlayer.transform.position);
            }
        }
    }
}