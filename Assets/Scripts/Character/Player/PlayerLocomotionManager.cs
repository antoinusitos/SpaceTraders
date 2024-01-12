using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AG
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        private PlayerManager player = null;

        [HideInInspector]
        public float verticalMovement = 0.0f;
        [HideInInspector]
        public float horizontalMovement = 0.0f;
        [HideInInspector]
        public float moveAmount = 0.0f;

        [Header("Movement Settings")]
        private Vector3 moveDirection = Vector3.zero;
        private Vector3 targetRotationDirection = Vector3.zero;
        [SerializeField]
        private float walkingSpeed = 2.0f;
        [SerializeField]
        private float runningSpeed = 5.0f;
        [SerializeField]
        private float spritingSpeed = 6.5f;
        [SerializeField]
        private float rotationSpeed = 15.0f;
        [SerializeField]
        private int sprintingStaminaCost = 2;
        [SerializeField]
        private float crouchingSpeed = 1.0f;

        [Header("Dodge")]
        private Vector3 rollDirection = Vector3.zero;
        [SerializeField]
        private float dodgeStaminaCost = 25;

        [Header("Jump")]
        [SerializeField]
        private float jumpStaminaCost = 25;
        [SerializeField]
        private float jumpHeight = 4.0f;
        [SerializeField]
        private float jumpForwardSpeed = 5.0f;
        private float freeFallSpeed = 2.0f;
        private Vector3 jumpDirection = Vector3.zero;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();

            player.animator.SetBool("IsCrouching", player.playerNetworkManager.isCrouching.Value);

            if (player.IsOwner)
            {
                player.characterNetworkManager.verticalMovement.Value = verticalMovement;
                player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
                player.characterNetworkManager.moveAmount.Value = moveAmount;
            }
            else
            {
                verticalMovement = player.characterNetworkManager.verticalMovement.Value;
                horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
                moveAmount = player.characterNetworkManager.moveAmount.Value;

                player.playerAnimatorManager.UpdateAnimatorValuesParamaters(horizontalMovement, verticalMovement, player.playerNetworkManager.isSprinting.Value);
            }
        }

        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
            HandleJumpingMovement();
            HandleFreeFallMovement();
        }

        private void GetMovementValues()
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
            moveAmount = PlayerInputManager.instance.moveAmount;
        }

        private void HandleGroundedMovement()
        {
            if (!player.canMove)
            {
                return;
            }

            GetMovementValues();

            moveDirection = transform.forward * verticalMovement;
            moveDirection += transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;

            if(player.playerNetworkManager.isSprinting.Value)
            {
                player.characterController.Move(moveDirection * spritingSpeed * Time.deltaTime);
            }
            else if(player.playerNetworkManager.isCrouching.Value)
            {
                player.characterController.Move(moveDirection * crouchingSpeed * Time.deltaTime);
            }
            else
            {
                if (PlayerInputManager.instance.moveAmount > 0.5f)
                {
                    player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
                }
                else if (PlayerInputManager.instance.moveAmount <= 0.5f)
                {
                    player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
                }
            }
        }

        private void HandleJumpingMovement()
        {
            if(player.isJumping)
            {
                player.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
            }
        }

        private void HandleFreeFallMovement()
        {
            if(!player.isGrounded)
            {
                Vector3 freeFallDirection = Vector3.zero;

                freeFallDirection = player.playerFPSCamera.transform.forward * PlayerInputManager.instance.verticalInput;
                freeFallDirection += player.playerFPSCamera.transform.right * PlayerInputManager.instance.horizontalInput;
                freeFallDirection.y = 0;

                player.characterController.Move(freeFallDirection * freeFallSpeed * Time.deltaTime);
            }
        }

        private void HandleRotation()
        {
            return;

            if(!player.canRotate)
            {
                return;
            }

            targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;

            if (targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }

            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }

        public void HandleSprinting()
        {
            if(player.isPerformingAction)
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }

            if(player.playerNetworkManager.currentStamina.Value <= 0)
            {
                player.playerNetworkManager.isSprinting.Value = false;
                return;
            }

            if(moveAmount >= 0.5f)
            {
                player.playerNetworkManager.isSprinting.Value = true;
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }

            if(player.playerNetworkManager.isSprinting.Value)
            {
                player.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
            }
        }

        public void AttemptToPerformDodge()
        {
            if(player.isPerformingAction)
            {
                return;
            }

            if(player.playerNetworkManager.currentStamina.Value <= 0)
            {
                return;
            }

            if(PlayerInputManager.instance.moveAmount > 0)
            {
                rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
                rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
                rollDirection.y = 0;
                rollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;

                player.playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true, true);
            }
            else
            {
                player.playerAnimatorManager.PlayTargetActionAnimation("Back_Step_01", true, true);
            }

            player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;
        }

        public void AttemptToPerformJump()
        {
            if (player.isPerformingAction)
            {
                return;
            }

            if(player.isJumping)
            {
                return;
            }

            if (!player.isGrounded)
            {
                return;
            }

            player.playerNetworkManager.isCrouching.Value = false;

            player.playerAnimatorManager.PlayTargetActionAnimation("JumpStart", false);

            player.isJumping = true;

            jumpDirection = player.playerFPSCamera.transform.forward * PlayerInputManager.instance.verticalInput;
            jumpDirection += player.playerFPSCamera.transform.right * PlayerInputManager.instance.horizontalInput;
            jumpDirection.y = 0;

            if(jumpDirection != Vector3.zero)
            {
                if (player.playerNetworkManager.isSprinting.Value)
                {
                    jumpDirection *= 1;
                }
                else if (PlayerInputManager.instance.moveAmount > 0.5f)
                {
                    jumpDirection *= 0.5f;
                }
                else if (PlayerInputManager.instance.moveAmount < 0.5f)
                {
                    jumpDirection *= 0.25f;
                }
            }
        }

        public void ApplyJumpingVelocity()
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }
    }
}