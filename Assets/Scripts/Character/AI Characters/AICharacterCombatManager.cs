using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class AICharacterCombatManager : CharacterCombatManager
    {
        [Header("Action Recovery")]
        public float actionRecoveryTimer = 0;

        [Header("Target Information")]
        public float distanceFromTarget = 0;
        public float viewableAngle = 0;
        public Vector3 targetDirection = Vector3.zero;

        [Header("Detection")]
        [SerializeField]
        private float detectionRadius = 15;
        public float minimumFOV = -35;
        public float maximumFOV = 35;

        [Header("Attack Rotation Speed")]
        public float attackRotationSpeed = 25;

        public CharacterManager target = null;

        public void FindATargetViaLineOfSight(AICharacterManager aICharacter)
        {
            if(target != null)
            {
                return;
            }

            Collider[] colliders = Physics.OverlapSphere(aICharacter.transform.position, detectionRadius, WorldUtilityManager.instance.GetCharacterLayers());

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();
                if(targetCharacter == null )
                {
                    continue;
                }

                if (targetCharacter == aICharacter)
                {
                    continue;
                }

                if (targetCharacter.isDead.Value)
                {
                    continue;
                }

                Vector3 targetsDirection = targetCharacter.transform.position - aICharacter.transform.position;
                float angleOfPotentionTarget = Vector3.Angle(targetsDirection, aICharacter.transform.forward);
                if(angleOfPotentionTarget > minimumFOV && angleOfPotentionTarget < maximumFOV)
                {
                    if(Physics.Linecast(aICharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position, WorldUtilityManager.instance.GetEnviroLayers()))
                    {
                        Debug.DrawLine(aICharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position);
                    }
                    else
                    {
                        targetDirection = targetCharacter.transform.position - transform.position;
                        viewableAngle = WorldUtilityManager.instance.GetAngleOfTarget(transform, targetDirection);
                        aICharacter.characterCombatManager.SetTarget(targetCharacter);
                        PivotTowardsTarget(aICharacter);
                    }
                }
            }
        }

        public void PivotTowardsTarget(AICharacterManager aICharacter)
        {
            if(aICharacter.isPerformingAction)
            {
                return;
            }

            /*if(viewableAngle >= 20 && viewableAngle <= 60)
            {
                aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_45", true);
            }
            else if(viewableAngle <= -20 && viewableAngle >= -60)
            {
                aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_45", true);
            }
            else if (viewableAngle >= 61 && viewableAngle <= 110)
            {
                aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_90", true);
            }
            else if (viewableAngle <= -61 && viewableAngle >= -110)
            {
                aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_90", true);
            }
            if (viewableAngle >= 110 && viewableAngle <= 145)
            {
                aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_135", true);
            }
            else if (viewableAngle <= -110 && viewableAngle >= -145)
            {
                aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_135", true);
            }
            if (viewableAngle >= 146 && viewableAngle <= 180)
            {
                aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_180", true);
            }
            else if (viewableAngle <= -146 && viewableAngle >= -180)
            {
                aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_180", true);
            }*/
        }

        public void RotateTowardsAgent(AICharacterManager aICharacter)
        {
            if(aICharacter.aICharacterNetworkManager.isMoving.Value)
            {
                aICharacter.transform.rotation = aICharacter.navMeshAgent.transform.rotation;
            }
        }

        public void RotateTowardsTargetWhilstAttacking(AICharacterManager aICharacter)
        {
            if(currentTarget == null)
            {
                return;
            }

            if (!aICharacter.canRotate)
            {
                return;
            }

            if(!aICharacter.isPerformingAction)
            {
                return;
            }

            Vector3 targetDirection = currentTarget.transform.position - aICharacter.transform.position;
            targetDirection.y = 0;
            targetDirection.Normalize();

            if(targetDirection == Vector3.zero)
            {
                targetDirection = aICharacter.transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            aICharacter.transform.rotation = Quaternion.Slerp(aICharacter.transform.rotation, targetRotation, attackRotationSpeed * Time.deltaTime);
        }

        public void HandleActionRecovery(AICharacterManager aICharacter)
        {
            if(actionRecoveryTimer > 0)
            {
                if(!aICharacter.isPerformingAction)
                {
                    actionRecoveryTimer -= Time.deltaTime;
                }
            }
        }
    }
}