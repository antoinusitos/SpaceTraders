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

        public void SetNewStaminaValue(float oldValue, float newValue)
        {
            staminaBar.SetStat(newValue);
        }

        public void SetMaxStaminaValue(int maxStamina)
        {
            staminaBar.SetMaxStat(maxStamina);
        }

        private void Update()
        {
            if(!player)
            {
                return;
            }

            if (player.characterNetworkManager.currentStamina.Value >= player.characterNetworkManager.maxStamina.Value)
            {
                staminaBar.gameObject.SetActive(false);
            }
            else
            {
                staminaBar.gameObject.SetActive(true);
            }
        }
    }
}