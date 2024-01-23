using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.TextCore.Text;
using System;

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
        [HideInInspector]
        public CharacterInventoryManager characterInventoryManager = null;
        [HideInInspector]
        public CharacterCraftManager characterCraftManager = null;
        [HideInInspector]
        public CharacterInteractionManager characterInteractionManager = null;

        [Header("Flags")]
        public bool isPerformingAction = false;
        public bool canRotate = true;
        public bool canMove = true;
        public bool isJumping = false;
        public bool isGrounded = false;
        public bool isDead = false;
        public bool isInMenu = false;
        public bool isUsingAnInteractable = false;

        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);

            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterInventoryManager = GetComponent<CharacterInventoryManager>();
            characterCraftManager = GetComponent<CharacterCraftManager>();
            characterInteractionManager = GetComponent<CharacterInteractionManager>();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            characterNetworkManager.currentHealth.OnValueChanged += OnCurrentHealthChange;
        }

        private void OnCurrentHealthChange(float previousValue, float newValue)
        {
            if(!IsOwner)
            {
                return;
            }

            if(newValue <= 0)
            {
                OnDeath();
            }
        }

        protected virtual void Update()
        {
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("InAirTimer", characterNetworkManager.inAirTimer.Value);

            if (IsOwner)
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

        protected virtual void OnDeath()
        {
            characterAnimatorManager.PlayTargetActionAnimation("Death", true);

            isDead = true;
            canMove = false;
            canRotate = false;
            isInMenu = false;
            PlayerCamera.instance.SetupCamera();
            PlayerUIManager.instance.CloseMenus();
        }
    }
}