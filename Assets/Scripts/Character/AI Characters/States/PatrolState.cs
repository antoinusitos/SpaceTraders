using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AG
{
    [CreateAssetMenu(menuName = "AI/States/Patrol")]
    public class PatrolState : AIState
    {
        private bool hasTakenDestination = false;
        private Vector3 destination = Vector3.zero;

        private Transform[] patrolPoints = null;

        public override AIState Tick(AICharacterManager aICharacter)
        {
            aICharacter.navMeshAgent.speed = aICharacter.walkingSpeed;

            aICharacter.aICharacterNetworkManager.isMoving.Value = true;

            if (aICharacter.characterCombatManager.currentTarget != null)
            {
                return SwitchState(aICharacter, aICharacter.roarState);
            }
            
            if(patrolPoints == null || patrolPoints.Length == 0)
            {
                Transform patrolPointsParent = FindObjectOfType<PatrolPoints>().transform;
                patrolPoints = new Transform[patrolPointsParent.childCount];
                for (int i = 0; i < patrolPointsParent.childCount; i++)
                {
                    patrolPoints[i] = patrolPointsParent.GetChild(i).transform;
                }
            }

            if (!hasTakenDestination)
            {
                hasTakenDestination = true;
                destination = patrolPoints[Random.Range(0, patrolPoints.Length)].position;
            }

            aICharacter.transform.position = aICharacter.navMeshAgent.transform.position;
            aICharacter.transform.rotation = aICharacter.navMeshAgent.transform.rotation;

            NavMeshPath path = new NavMeshPath();
            aICharacter.navMeshAgent.CalculatePath(destination, path);
            aICharacter.navMeshAgent.SetPath(path);

            Debug.DrawLine(aICharacter.transform.position, destination);

            if (Vector3.Distance(aICharacter.transform.position, destination) <= aICharacter.navMeshAgent.stoppingDistance)
            {
                return SwitchState(aICharacter, aICharacter.idle);
            }

            return this;
        }

        protected override void ResetStateFlags(AICharacterManager aICharacter)
        {
            base.ResetStateFlags(aICharacter);

            hasTakenDestination = false;
            patrolPoints = null;
        }
    }
}