using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "AI/Actions/Attack")]
    public class AICharacterAttackAction : ScriptableObject
    {
        [Header("Attack")]
        [SerializeField]
        private string attackAnimation = "";

        [Header("Combo Action")]
        public AICharacterAttackAction comboAction = null;

        [Header("Action Values")]
        public int attackWeight = 50;
        public float actionRecoveryTime = 1.5f;
        public float minimumAttackAngle = -35;
        public float maximumAttackAngle = 35;
        public float minimumAttackDistance = 0;
        public float maximumAttackDistance = 2;

        public void AttemptToPerformAction(AICharacterManager aICharacter)
        {
            aICharacter.characterAnimatorManager.PlayTargetAttackActionAnimation(attackAnimation, true);
        }
    }
}