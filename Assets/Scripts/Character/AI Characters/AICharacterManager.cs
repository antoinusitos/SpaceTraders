using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AG
{
    public class AICharacterManager : CharacterManager
    {
        [HideInInspector]
        public AICharacterNetworkManager aICharacterNetworkManager = null;
        [HideInInspector]
        public AICharacterCombatManager aICharacterCombatManager = null;

        [Header("Current State")]
        [SerializeField]
        private AIState currentState = null;

        [Header("Navmesh Agent")]
        public NavMeshAgent navMeshAgent = null;

        [Header("States")]
        public IdleState idle = null;
        public PursueTargetState pursueTarget = null;


        protected override void Awake()
        {
            base.Awake();

            aICharacterCombatManager = GetComponent<AICharacterCombatManager>();
            aICharacterNetworkManager = GetComponent<AICharacterNetworkManager>();

            navMeshAgent = GetComponentInChildren<NavMeshAgent>();

            idle = Instantiate(idle);
            pursueTarget = Instantiate(pursueTarget);

            currentState = idle;
        }

        protected override void FixedUpdate()
        {
            ProcessStateMachine();
        }

        private void ProcessStateMachine()
        {
            AIState nextState = currentState?.Tick(this);

            if (nextState != null)
            {
                currentState = nextState;
            }

            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;

            if (navMeshAgent.enabled)
            {
                Vector3 agentDestination = navMeshAgent.destination;
                float remainingDistance = Vector3.Distance(agentDestination, transform.position);

                if(remainingDistance > navMeshAgent.stoppingDistance)
                {
                    aICharacterNetworkManager.isMoving.Value = true;
                }
                else
                {
                    aICharacterNetworkManager.isMoving.Value = false;
                }
            }
            else
            {
                    aICharacterNetworkManager.isMoving.Value = false;
            }
        }
    }
}