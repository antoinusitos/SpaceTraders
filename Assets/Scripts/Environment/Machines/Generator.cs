using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class Generator : Machine
    {
        [SerializeField]
        private float minSliderValue = -345.0f;
        [SerializeField]
        private float maxSliderValue = 345.0f;
        [SerializeField]
        private float valueSpeed = 1000.0f;
        [SerializeField]
        private RectTransform sliderImage = null;
        [SerializeField]
        private RectTransform sliderRangeImage = null;
        [SerializeField]
        private Color activeStateColor = Color.yellow;
        [SerializeField]
        private Color inactiveStateColor = Color.yellow;


        [SerializeField]
        private float firstSize = 300.0f;
        [SerializeField]
        private float secondSize = 200.0f;
        [SerializeField]
        private float thirdSize = 100.0f;
        [SerializeField]
        private Image firstImage = null;
        [SerializeField]
        private Image secondImage = null;
        [SerializeField]
        private Image thirdImage = null;

        private float currentValue = 0;
        private float direction = 1;
        private bool move = false;

        private int state = 1;

        public override bool OnInteract(PlayerInteractionManager playerUsing)
        {
            if (isDone.Value)
            {
                return false;
            }

            if (!base.OnInteract(playerUsing))
            {
                return false;
            }

            player = playerUsing;

            currentValue = minSliderValue;
            move = true;

            state = 1;

            sliderRangeImage.sizeDelta = new Vector2 (firstSize, 50);

            return true;
        }

        public override void OnStopInteract()
        {
            base.OnStopInteract();

            move = false;
            player = null;
        }

        private void Update()
        {
            if(!move)
            { 
                return;
            }

            currentValue += direction * valueSpeed * Time.deltaTime;
            if(currentValue > maxSliderValue)
            {
                currentValue = maxSliderValue;
                direction = -1;
            }
            else if (currentValue < minSliderValue)
            {
                currentValue = minSliderValue;
                direction = 1;
            }

            sliderImage.anchoredPosition = new Vector2(currentValue, 0);
        }

        private void UpdateSliders()
        {
            currentValue = minSliderValue;

            if (state == 1)
            {
                sliderRangeImage.sizeDelta = new Vector2(firstSize, 50);
                firstImage.color = inactiveStateColor;
                secondImage.color = inactiveStateColor;
                thirdImage.color = inactiveStateColor;
            }
            else if (state == 2)
            {
                sliderRangeImage.sizeDelta = new Vector2(secondSize, 50);
                firstImage.color = activeStateColor;
                secondImage.color = inactiveStateColor;
                thirdImage.color = inactiveStateColor;
            }
            else if (state == 3)
            {
                sliderRangeImage.sizeDelta = new Vector2(thirdSize, 50);
                firstImage.color = activeStateColor;
                secondImage.color = activeStateColor;
                thirdImage.color = inactiveStateColor;
            }
        }

        public void OnButtonClicked()
        {
            if(isDone.Value)
            {
                return;
            }

            if(state == 1)
            {
                if(currentValue >= -firstSize / 2 && currentValue <= firstSize / 2)
                {
                    state++;
                }
                UpdateSliders();
            }
            else if (state == 2)
            {
                if (currentValue >= -secondSize / 2 && currentValue <= secondSize / 2)
                {
                    state++;
                }
                else
                {
                    state--;
                }
                UpdateSliders();
            }
            else if (state == 3)
            {
                if (currentValue >= -thirdSize / 2 && currentValue <= thirdSize / 2)
                {
                    move = false;
                    player.SetIsDoneOnMachine(NetworkObject.NetworkObjectId);
                    thirdImage.color = activeStateColor;
                    player.StopInteract();
                }
                else
                {
                    state--;
                    UpdateSliders();
                }
            }
        }
    }
}