using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "AI/States/Idle")]
    public class IdleState : AIState
    {
        public override AIState Tick(AICharacterManager aICharacter)
        {
            if(aICharacter.characterCombatManager.currentTarget != null)
            {
                return SwitchState(aICharacter, aICharacter.pursueTarget);
            }
            else
            {
                aICharacter.aICharacterCombatManager.FindATargetViaLineOfSight(aICharacter);
                return this;
            }
        }
    }
}