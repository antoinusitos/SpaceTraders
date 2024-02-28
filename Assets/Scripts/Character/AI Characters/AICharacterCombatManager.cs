using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class AICharacterCombatManager : CharacterCombatManager
    {
        [Header("Detection")]
        [SerializeField]
        private float detectionRadius = 15;
        [SerializeField]
        private float minimumDetectionAngle = -35;
        [SerializeField]
        private float maximumDetectionAngle = 35;

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
                float viewableAngle = Vector3.Angle(targetsDirection, aICharacter.transform.forward);
                if(viewableAngle > minimumDetectionAngle && viewableAngle < maximumDetectionAngle)
                {
                    if(Physics.Linecast(aICharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position, WorldUtilityManager.instance.GetEnviroLayers()))
                    {
                        Debug.DrawLine(aICharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position);
                        Debug.Log("BLOCKED");
                    }
                    else
                    {
                        aICharacter.characterCombatManager.SetTarget(targetCharacter);
                    }
                }
            }
        }
    }
}