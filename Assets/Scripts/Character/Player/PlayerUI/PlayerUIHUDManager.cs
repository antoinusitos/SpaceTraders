using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace AG
{
    public class PlayerUIHUDManager : MonoBehaviour
    {
        [HideInInspector]
        public PlayerManager player = null;

        [SerializeField]
        private UI_StatBar staminaBar = null;
        [SerializeField]
        private UI_StatBar healthBar = null;

        public void SetNewHealthValue(float oldValue, float newValue)
        {
            healthBar.SetStat(newValue);
        }

        public void SetMaxHealthValue(int maxHealth)
        {
            healthBar.SetMaxStat(maxHealth);
        }

        public void SetNewStaminaValue(float oldValue, float newValue)
        {
            staminaBar.SetStat(newValue);
            if (player.characterNetworkManager.currentStamina.Value >= player.characterNetworkManager.maxStamina.Value)
            {
                staminaBar.gameObject.SetActive(false);
            }
            else
            {
                staminaBar.gameObject.SetActive(true);
            }
        }

        public void SetMaxStaminaValue(int maxStamina)
        {
            staminaBar.SetMaxStat(maxStamina);
        }

        public void RefreshHUD()
        {
            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);
            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);
        }
    }
}