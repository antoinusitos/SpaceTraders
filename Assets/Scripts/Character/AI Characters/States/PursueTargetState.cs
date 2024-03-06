using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AG
{
    [CreateAssetMenu(menuName = "AI/States/Pursue Target")]
    public class PursueTargetState : AIState
    {
        public override AIState Tick(AICharacterManager aICharacter)
        {
            aICharacter.navMeshAgent.speed = aICharacter.runninngSpeed;

            if (aICharacter.isPerformingAction)
            {
                return this;
            }

            if(aICharacter.aICharacterCombatManager.currentTarget == null)
            {
                return SwitchState(aICharacter, aICharacter.idle);
            }

            if(!aICharacter.navMeshAgent.enabled)
            {
                aICharacter.navMeshAgent.enabled = true;
            }

            if(aICharacter.aICharacterCombatManager.viewableAngle < aICharacter.aICharacterCombatManager.minimumFOV || 
                aICharacter.aICharacterCombatManager.viewableAngle > aICharacter.aICharacterCombatManager.maximumFOV)
            {
                aICharacter.aICharacterCombatManager.PivotTowardsTarget(aICharacter);
            }

            aICharacter.transform.position = aICharacter.navMeshAgent.transform.position;
            aICharacter.transform.rotation = aICharacter.navMeshAgent.transform.rotation;

            if(aICharacter.aICharacterCombatManager.distanceFromTarget <= aICharacter.navMeshAgent.stoppingDistance)
            {
                return SwitchState(aICharacter, aICharacter.attack);
            }

            NavMeshPath path = new NavMeshPath();
            aICharacter.navMeshAgent.CalculatePath(aICharacter.aICharacterCombatManager.currentTarget.transform.position, path);
            aICharacter.navMeshAgent.SetPath(path);

            return this;
        }
    }
}