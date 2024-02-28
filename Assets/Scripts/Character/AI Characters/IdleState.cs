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
                Debug.Log("We have a target");
                return this;
            }
            else
            {
                aICharacter.aICharacterCombatManager.FindATargetViaLineOfSight(aICharacter);
                Debug.Log("Searching a target");
                return this;
            }
        }
    }
}