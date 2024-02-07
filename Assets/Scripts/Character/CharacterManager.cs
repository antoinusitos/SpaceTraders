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
        [Header("Status")]
        public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

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
        [HideInInspector]
        public CharacterEffectsManager characterEffectsManager = null;

        [Header("Flags")]
        public bool isPerformingAction = false;
        public bool canRotate = true;
        public bool canMove = true;
        public bool isGrounded = false;
        public bool isInMenu = false;
        public bool isUsingAnInteractable = false;

        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);

            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterInventoryManager = GetComponent<CharacterInventoryManager>();
            characterCraftManager = GetComponent<CharacterCraftManager>();
            characterInteractionManager = GetComponent<CharacterInteractionManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
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
                if (newValue <= 0)
                {
                    animator.enabled = false;
                }
                else
                {
                    animator.enabled = true;
                }
                return;
            }

            if(newValue <= 0)
            {
                OnDeath();
            }
            else if(isDead.Value)
            {
                OnRevive();
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

        protected virtual void FixedUpdate()
        {

        }

        protected virtual void LateUpdate()
        {

        }

        protected virtual void OnDeath()
        {
            isDead.Value = true;
            canMove = false;
            canRotate = false;
            isInMenu = false;
            PlayerCamera.instance.SetupCamera();
            PlayerUIManager.instance.CloseMenus();
        }

        protected virtual void OnRevive()
        {
            isDead.Value = false;
            canMove = true;
            canRotate = true;
            isInMenu = false;
            PlayerCamera.instance.StopCamera();
        }
    }
}