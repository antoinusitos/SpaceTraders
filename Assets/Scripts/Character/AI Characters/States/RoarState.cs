using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "AI/States/Roar")]
    public class RoarState : AIState
    {
        [SerializeField]
        private string roarAnimation = "";

        private bool hasRoared = false;

        public override AIState Tick(AICharacterManager aICharacter)
        {
            if(aICharacter.isPerformingAction)
            {
                return this;
            }

            if(!hasRoared)
            {
                hasRoared = true;
                aICharacter.characterAnimatorManager.PlayTargetActionAnimation(roarAnimation, true, false, false);
                return this;
            }

            return SwitchState(aICharacter, aICharacter.pursueTarget);
        }

        protected override void ResetStateFlags(AICharacterManager aICharacter)
        {
            base.ResetStateFlags(aICharacter);

            aICharacter.canMove = true;
            aICharacter.canRotate = true;
            hasRoared = false;
        }
    }
}