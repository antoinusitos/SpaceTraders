using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace AG
{
    public class CharacterStatsManager : MonoBehaviour
    {
        private CharacterManager character = null;

        [Header("Stamine Regeneration")]
        [SerializeField]
        private float staminaRegenerationAmount = 5;
        private float staminaRegenerationTimer = 0;
        private float staminaTickTimer = 0;
        [SerializeField]
        private float staminaRegenerationDelay = 2.0f;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {
            float stamina = 0;

            stamina = endurance * 10;

            return Mathf.RoundToInt(stamina);
        }

        public virtual void RegenerateStamina()
        {
            if (!character.IsOwner)
            {
                return;
            }

            if (character.characterNetworkManager.isSprinting.Value)
            {
                return;
            }

            if (character.isPerformingAction)
            {
                return;
            }

            staminaRegenerationTimer += Time.deltaTime;

            if (staminaRegenerationTimer >= staminaRegenerationDelay)
            {
                if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
                {
                    character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount * Time.deltaTime;
                    /*staminaTickTimer += Time.deltaTime;

                    if (staminaTickTimer >= 0.1f)
                    {
                        staminaTickTimer = 0;
                    }*/
                }
            }
        }

        public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount)
        {
            if(currentStaminaAmount < previousStaminaAmount)
            {
                staminaRegenerationTimer = 0;
            }
        }
    }
}