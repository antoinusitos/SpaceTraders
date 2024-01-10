using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PlayerUIHUDManager : MonoBehaviour
    {
        [SerializeField]
        private UI_StatBar staminaBar = null;

        public void SetNewStaminaValue(float oldValue, float newValue)
        {
            staminaBar.SetStat(Mathf.RoundToInt(newValue));
        }

        public void SetMaxStaminaValue(int maxStamina)
        {
            staminaBar.SetMaxStat(maxStamina);
        }
    }
}