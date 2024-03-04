using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "AI/States/Attack")]
    public class AttackState : AIState
    {
        [HideInInspector]
        public AICharacterAttackAction currentAttack = null;
        [HideInInspector]
        public bool willPerformCombo = false;

        [Header("State Flags")]
        protected bool hasPerformedAttack = false;
        protected bool hasPerformedCombo = false;

        [Header("Pivot After Attack")]
        [SerializeField]
        protected bool pivotAfterAttack = false;

        public override AIState Tick(AICharacterManager aICharacter)
        {
            if(aICharacter.aICharacterCombatManager.currentTarget == null)
            {
                return SwitchState(aICharacter, aICharacter.idle);
            }

            if(aICharacter.aICharacterCombatManager.currentTarget.isDead.Value)
            {
                return SwitchState(aICharacter, aICharacter.idle);
            }

            aICharacter.characterAnimatorManager.UpdateAnimatorValuesParamaters(0, 0, false);

            if(willPerformCombo && !hasPerformedCombo)
            {
                if(currentAttack.comboAction != null)
                {
                    //hasPerformedCombo = true;
                    //currentAttack.comboAction.AttemptToPerformAction(aICharacter);
                }
            }

            if(!hasPerformedAttack)
            {
                if (aICharacter.aICharacterCombatManager.actionRecoveryTimer > 0)
                {
                    return this;
                }

                if(aICharacter.isPerformingAction)
                {
                    return this;
                }

                PerformAttack(aICharacter);

                return this;
            }

            if(pivotAfterAttack)
            {
                aICharacter.aICharacterCombatManager.PivotTowardsTarget(aICharacter);
            }

            return SwitchState(aICharacter, aICharacter.combatStance);
        }

        protected void PerformAttack(AICharacterManager aICharacter)
        {
            hasPerformedAttack = true;
            currentAttack.AttemptToPerformAction(aICharacter);
            aICharacter.aICharacterCombatManager.actionRecoveryTimer = currentAttack.actionRecoveryTime;
        }

        protected override void ResetStateFlags(AICharacterManager aICharacter)
        {
            base.ResetStateFlags(aICharacter);

            hasPerformedAttack = false;
            hasPerformedCombo = false;
        }
    }
}