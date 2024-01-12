using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace AG
{
    public class CharacterManager : NetworkBehaviour
    {
        [HideInInspector]
        public CharacterController characterController = null;
        [HideInInspector]
        public Animator animator = null;
        [HideInInspector]
        public CharacterNetworkManager characterNetworkManager = null;
        [HideInInspector]
        public CharacterAnimatorManager characterAnimatorManager = null;

        [Header("Flags")]
        public bool isPerformingAction = false;
        public bool canRotate = true;
        public bool canMove = true;
        public bool isJumping = false;
        public bool isGrounded = false;

        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);

            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        }

        protected virtual void Update()
        {
            animator.SetBool("IsGrounded", isGrounded);

            if(IsOwner)
            {
                characterNetworkManager.networkPosition.Value = transform.position;
                characterNetworkManager.networkRotation.Value = transform.rotation;
            }
            else
            {
                transform.position = Vector3.SmoothDamp(
                    transform.position, 
                    characterNetworkManager.networkPosition.Value, 
                    ref characterNetworkManager.networkPositionVelocity, 
                    characterNetworkManager.networkPositionSmoothTime);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    characterNetworkManager.networkRotation.Value,
                    characterNetworkManager.networkRotationSmoothTime);
            }
        }

        protected virtual void LateUpdate()
        {

        }
    }
}