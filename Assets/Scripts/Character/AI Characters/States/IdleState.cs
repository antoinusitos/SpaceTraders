using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "AI/States/Idle")]
    public class IdleState : AIState
    {
        public float idleTime = 5;
        private float currentIdleTime = 0;

        public override AIState Tick(AICharacterManager aICharacter)
        {
            if (aICharacter.characterCombatManager.currentTarget != null)
            {
                return SwitchState(aICharacter, aICharacter.roarState);
            }
            else
            {
                aICharacter.aICharacterCombatManager.FindATargetViaLineOfSight(aICharacter);
                aICharacter.aICharacterNetworkManager.isMoving.Value = false;

                currentIdleTime += Time.deltaTime;
                if(currentIdleTime > idleTime)
                {
                    return SwitchState(aICharacter, aICharacter.patrolState);
                }

                return this;
            }
        }

        protected override void ResetStateFlags(AICharacterManager aICharacter)
        {
            base.ResetStateFlags(aICharacter);

            currentIdleTime = 0;
        }
    }
}