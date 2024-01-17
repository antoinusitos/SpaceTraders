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
        [SerializeField]
        private float staminaRegenerationDelay = 2.0f;

        [SerializeField]
        private float vitalityMultiplier = 10;
        [SerializeField]
        private float enduranceMultiplier = 10;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Start()
        {

        }

        public int CalculateHealthBasedOnVitalityLevel(int vitality)
        {
            float health = 0;

            health = vitality * vitalityMultiplier;

            return Mathf.RoundToInt(health);
        }

        public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {
            float stamina = 0;

            stamina = endurance * enduranceMultiplier;

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