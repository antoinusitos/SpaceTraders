using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class UI_StatBar : MonoBehaviour
    {
        private Slider slider = null;
        private RectTransform rectTransform = null;

        [Header("Bar Options")]
        [SerializeField]
        protected bool scaleBarWithStats = true;
        [SerializeField]
        protected float widthScaleMultiplier = 2.0f;

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
            rectTransform = GetComponent<RectTransform>();
        }

        public virtual void SetStat(float newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxStat(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;

            if(scaleBarWithStats)
            {
                rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y);
                PlayerUIManager.instance.playerUIHUDManager.RefreshHUD();
            }
        }
    }
}