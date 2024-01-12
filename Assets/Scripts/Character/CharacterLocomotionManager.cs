using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        private CharacterManager character = null;

        [Header("Ground Check & Jumping")]
        [SerializeField]
        protected float gravityForce = -5.55f;
        [SerializeField]
        private float groundCheckSphereRadius = 1.0f;
        [SerializeField]
        private LayerMask groundLayer;
        [SerializeField]
        protected Vector3 yVelocity = Vector3.zero;
        [SerializeField]
        protected float groundedYVelocity = -20.0f;
        [SerializeField]
        protected float fallStartYVelocity = -5.0f;
        protected bool fallingVelocityHasBeenSet = false;
        protected float inAirTimer = 0f;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {
            HandleGroundCheck();
            if(character.isGrounded)
            {
                if(yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                if(!character.isJumping && !fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }
                inAirTimer += Time.deltaTime;
                character.animator.SetFloat("InAirTimer", inAirTimer);

                yVelocity.y += gravityForce * Time.deltaTime;
            }

            character.characterController.Move(yVelocity * Time.deltaTime);
        }

        protected void HandleGroundCheck()
        {
            character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
        }

        protected void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
        }
    }
}