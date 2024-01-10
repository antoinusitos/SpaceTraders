using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AG
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        private CharacterManager character = null;

        private int vertical = 0;
        private int horizontal = 0;

        public readonly float crossFadeTime = 0.2f;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();

            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValuesParamaters(float horizontalValue, float verticalValues, bool isSprinting)
        {
            float horizontalAmount = horizontalValue;
            float verticalAmount = verticalValues;

            if(isSprinting)
            {
                verticalAmount = 2;
            }

            character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
            character.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);
        }

        public virtual void PlayTargetActionAnimation(
            string targetAnimation, 
            bool isPerformingAction, 
            bool applyRootMotion = true, 
            bool canRotate = false, 
            bool canMove = false)
        {
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, crossFadeTime);
            character.isPerformingAction = isPerformingAction;
            character.canRotate = canRotate;
            character.canMove = canMove;

            character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }
    }
}