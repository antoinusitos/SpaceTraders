using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        private PlayerManager player = null;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        private void OnAnimatorMove()
        {
            if(player.applyRootMotion)
            {
                Vector3 velocity = player.animator.deltaPosition;
                player.characterController.Move(velocity);
                player.transform.rotation *= player.animator.deltaRotation;
            }
        }
    }
}