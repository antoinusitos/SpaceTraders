using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        private PlayerManager player = null;

        public WeaponItem currentRightHandWeapon = null;

        private float flashLightDuration = 20.0f;
        private float currentFlashLightDuration = 0.0f;

        private bool usingFlashLight = false;

        private float refillAmount = 0;
        private float refillTime = 50.0f;
        private float refillSpeed = 10.0f;

        [Header("Debug")]
        [SerializeField]
        private bool giveBattery = false;
        [SerializeField]
        private bool refillBattery = false;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
            currentFlashLightDuration = flashLightDuration;
        }

        public void FlashLightState(bool previousState, bool newState)
        {
            usingFlashLight = newState;
        }

        protected override void Update()
        {
            base.Update();

            if (usingFlashLight)
            {
                currentFlashLightDuration -= Time.deltaTime;
                PlayerUIManager.instance.playerUIHUDManager.UpdateFlashLightBatterie(currentFlashLightDuration, flashLightDuration);
                if (currentFlashLightDuration <= 0)
                {
                    player.characterNetworkManager.flashlightOn.Value = false;
                }
            }

            if(giveBattery)
            {
                giveBattery = false;
                flashlightBatteriesNumber++;
                PlayerUIManager.instance.playerUIHUDManager.UpdateFlashLightBatteriesNumber(flashlightBatteriesNumber);
            }

            if(refillBattery)
            {
                refillBattery = false;
                RefillBattery();
            }
        }

        public void ActivateBatterieRefill()
        {
            if(flashlightBatteriesNumber > 0)
            {
                refillAmount += Time.deltaTime * refillSpeed;
                if(refillAmount >= refillTime)
                {
                    refillAmount = 0;
                    RefillBattery();
                }
                PlayerUIManager.instance.playerUIHUDManager.UpdateRefillSlider(refillAmount / refillTime);
            }

        }

        public void StopBatterieRefill()
        {
            if(refillAmount > 0)
            {
                refillAmount -= Time.deltaTime * refillSpeed;
                if (refillAmount < 0)
                {
                    refillAmount = 0;
                }
                PlayerUIManager.instance.playerUIHUDManager.UpdateRefillSlider(refillAmount / refillTime);
            }
        }

        public void RefillBattery()
        {
            if(flashlightBatteriesNumber > 0)
            {
                flashlightBatteriesNumber--;
                PlayerUIManager.instance.playerUIHUDManager.UpdateFlashLightBatteriesNumber(flashlightBatteriesNumber);
                currentFlashLightDuration = flashLightDuration;
                PlayerUIManager.instance.playerUIHUDManager.UpdateFlashLightBatterie(currentFlashLightDuration, flashLightDuration);
            }
        }

        public void EmptyInventory()
        {
            flashlightBatteriesNumber = 0;
            PlayerUIManager.instance.playerUIHUDManager.UpdateFlashLightBatteriesNumber(flashlightBatteriesNumber);
            currentFlashLightDuration = flashLightDuration;
            PlayerUIManager.instance.playerUIHUDManager.UpdateFlashLightBatterie(currentFlashLightDuration, flashLightDuration);
            usingFlashLight = false;

            for(int i = 0; i < inventory.Length; i++)
            {
                inventory[i] = null;
            }

            scrapInventory.Clear();
        }
    }
}