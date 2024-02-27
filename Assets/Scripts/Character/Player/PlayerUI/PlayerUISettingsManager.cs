using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class PlayerUISettingsManager : MonoBehaviour
    {
        public TextMeshProUGUI fovValue = null;
        public Slider fovSlider = null;

        public InputField playerNameInputField = null;

        public TextMeshProUGUI horizontalSpeedValue = null;
        public Slider horizontalSpeedSlider = null;

        public TextMeshProUGUI verticalSpeedValue = null;
        public Slider verticalSpeedSlider = null;

        public Toggle invertYToggle = null;

        public void OnEnable()
        {
            RetrieveSettingsValues();
        }

        public void OnDisable()
        {
            RetrieveSettingsValues();
        }

        private void RetrieveSettingsValues()
        {
            float fov = WorldSettingsManager.instance.GetFOV();
            fovValue.text = fov.ToString();
            fovSlider.value = fov;

            string playerName = WorldSettingsManager.instance.GetPlayerName();
            playerNameInputField.text = playerName;

            float horizontalSpeed = WorldSettingsManager.instance.GetHorizontalCameraSpeed();
            horizontalSpeedValue.text = horizontalSpeed.ToString();
            horizontalSpeedSlider.value = horizontalSpeed;

            float verticalSpeed = WorldSettingsManager.instance.GetVerticalCameraSpeed();
            verticalSpeedValue.text = verticalSpeed.ToString();
            verticalSpeedSlider.value = verticalSpeed;

            invertYToggle.isOn = WorldSettingsManager.instance.GetInvertY();
        }

        public void OnFOVSliderChanged(float value)
        {
            WorldSettingsManager.instance.SetFOV(value);
            fovValue.text = value.ToString();
        }

        public void OnPlayerNameChanged()
        {
            WorldSettingsManager.instance.SetPlayerName(playerNameInputField.text);
        }

        public void OnHorizontalSpeedSliderChanged(float value)
        {
            WorldSettingsManager.instance.SetHorizontalCameraSpeed(value);
            horizontalSpeedValue.text = value.ToString();
        }

        public void OnVerticalSpeedSliderChanged(float value)
        {
            WorldSettingsManager.instance.SetVerticalCameraSpeed(value);
            verticalSpeedValue.text = value.ToString();
        }

        public void OnInvertYChanged()
        {
            WorldSettingsManager.instance.SetInvertY(invertYToggle.isOn);
        }

        public void SaveSettings()
        {
            WorldSettingsManager.instance.SaveSettings();
        }
    }
}