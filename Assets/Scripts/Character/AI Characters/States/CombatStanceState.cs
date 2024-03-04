using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AG
{
    [CreateAssetMenu(menuName = "AI/States/Combat State")]
    public class CombatStanceState : AIState
    {
        [Header("Attacks")]
        public List<AICharacterAttackAction> aiCharacterAttacks;
        private List<AICharacterAttackAction> potentialAttacks;
        private AICharacterAttackAction choosenAttack = null;
        private AICharacterAttackAction previousAttack = null;
        protected bool hasAttacked = false;

        [Header("Engagement Distance")]
        [SerializeField]
        protected float maximumEngagementDistance = 5.0f;

        public override AIState Tick(AICharacterManager aICharacter)
        {
            if(aICharacter.isPerformingAction)
            {
                return this;
            }

            if(!aICharacter.navMeshAgent.enabled)
            {
                aICharacter.navMeshAgent.enabled = true;
            }

            if(!aICharacter.aICharacterNetworkManager.isMoving.Value)
            {
                if(aICharacter.aICharacterCombatManager.viewableAngle < -30 || aICharacter.aICharacterCombatManager.viewableAngle > 30)
                {
                    aICharacter.aICharacterCombatManager.PivotTowardsTarget(aICharacter);
                }
            }

            aICharacter.aICharacterCombatManager.RotateTowardsAgent(aICharacter);

            if (aICharacter.aICharacterCombatManager.currentTarget == null)
            {
                return SwitchState(aICharacter, aICharacter.idle);
            }

            if(!hasAttacked)
            {
                GetNewAttack(aICharacter);
            }
            else
            {
                aICharacter.attack.currentAttack = choosenAttack;
                return SwitchState(aICharacter, aICharacter.attack);
            }

            if(aICharacter.aICharacterCombatManager.distanceFromTarget > maximumEngagementDistance)
            {
                return SwitchState(aICharacter, aICharacter.pursueTarget);
            }

            NavMeshPath path = new NavMeshPath();
            aICharacter.navMeshAgent.CalculatePath(aICharacter.aICharacterCombatManager.currentTarget.transform.position, path);
            aICharacter.navMeshAgent.SetPath(path);

            return this;
        }

        protected virtual void GetNewAttack(AICharacterManager aiCharacter)
        {
            potentialAttacks = new List<AICharacterAttackAction>();

            foreach(AICharacterAttackAction potientialAttack in aiCharacterAttacks)
            {
                if(potientialAttack.minimumAttackDistance > aiCharacter.aICharacterCombatManager.distanceFromTarget)
                {
                    continue;
                }

                if (potientialAttack.maximumAttackDistance < aiCharacter.aICharacterCombatManager.distanceFromTarget)
                {
                    continue;
                }

                if (potientialAttack.minimumAttackAngle > aiCharacter.aICharacterCombatManager.viewableAngle)
                {
                    continue;
                }

                if (potientialAttack.maximumAttackAngle < aiCharacter.aICharacterCombatManager.viewableAngle)
                {
                    continue;
                }

                potentialAttacks.Add(potientialAttack);
            }

            if(potentialAttacks.Count <= 0)
            {
                return;
            }

            int totalWeight = 0;

            foreach(AICharacterAttackAction attack in potentialAttacks)
            {
                totalWeight += attack.attackWeight;
            }

            float randomWeightValue = Random.Range(1, totalWeight + 1);
            int processedWeight = 0;

            foreach (AICharacterAttackAction attack in potentialAttacks)
            {
                processedWeight += attack.attackWeight;
                if(randomWeightValue <= processedWeight)
                {
                    choosenAttack = attack;
                    previousAttack = choosenAttack;
                    hasAttacked = true;
                }
            }
        }

        protected virtual void RollForOutcomeChance(int outcomeChance)
        {

        }

        protected override void ResetStateFlags(AICharacterManager aICharacter)
        {
            base.ResetStateFlags(aICharacter);
            hasAttacked = false;
        }
    }
}