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
            if(aICharacter.isPerformingAction)
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

            NavMeshPath path = new NavMeshPath();
            aICharacter.navMeshAgent.CalculatePath(aICharacter.aICharacterCombatManager.currentTarget.transform.position, path);
            aICharacter.navMeshAgent.SetPath(path);

            //aICharacter.transform.position = Vector3.Lerp(aICharacter.transform.position, aICharacter.navMeshAgent.transform.position, Time.deltaTime);
            aICharacter.transform.position = aICharacter.navMeshAgent.transform.position;// + Vector3.up * 1.7f;
            aICharacter.transform.rotation = aICharacter.navMeshAgent.transform.rotation;

            return this;
        }
    }
}