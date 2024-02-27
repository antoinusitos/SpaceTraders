using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public struct WorldSettings
    {
        public string playerName;
        public float currentFOV;
        public bool invertY;
        public float horizontalCameraSpeed;
        public float verticalCameraSpeed;
    }

    public class WorldSettingsManager : MonoBehaviour
    {
        public static WorldSettingsManager instance = null;

        private WorldSettings worldSettings;
        private WorldSettings worldSettingsBeforeSave;

        private SaveFileDataWriter saveFileDataWriter = null;

        public delegate void OnSettingChangedDelegate(float newValue);
        public delegate void OnStringSettingChangedDelegate(string newValue);
        public delegate void OnBoolSettingChangedDelegate(bool newValue);

        public OnStringSettingChangedDelegate OnPlayerNameChanged;
        public OnSettingChangedDelegate OnFOVChanged;
        public OnBoolSettingChangedDelegate OnInvertYChanged;
        public OnSettingChangedDelegate OnHorizontalCameraSpeedChanged;
        public OnSettingChangedDelegate OnVerticalCameraSpeedChanged;

        private void Awake()
        {
            if(instance)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                worldSettings = new WorldSettings()
                {
                    playerName = "Player Name",
                    currentFOV = 60.0f,
                    invertY = false,
                    horizontalCameraSpeed = 50,
                    verticalCameraSpeed = 50
                };
                LoadSettings();
            }
        }

        public void SetPlayerName(string newValue)
        {
            worldSettingsBeforeSave.playerName = newValue;
            if (OnPlayerNameChanged != null)
            {
                OnPlayerNameChanged.Invoke(newValue);
            }
        }

        public string GetPlayerName()
        {
            return  worldSettings.playerName; 
        }

        public void SetFOV(float newValue)
        {
            worldSettingsBeforeSave.currentFOV = newValue;
            if(OnFOVChanged != null)
            {
                OnFOVChanged.Invoke(newValue);
            }
        }

        public float GetFOV()
        {
            return worldSettings.currentFOV;
        }

        public void SetInvertY(bool newValue)
        {
            worldSettingsBeforeSave.invertY = newValue;
            if (OnInvertYChanged != null)
            {
                OnInvertYChanged.Invoke(newValue);
            }
        }

        public bool GetInvertY()
        {
            return worldSettings.invertY;
        }

        public void SetHorizontalCameraSpeed(float newValue)
        {
            worldSettingsBeforeSave.horizontalCameraSpeed = newValue;
            if (OnHorizontalCameraSpeedChanged != null)
            {
                OnHorizontalCameraSpeedChanged.Invoke(newValue);
            }
        }

        public float GetHorizontalCameraSpeed()
        {
            return worldSettings.horizontalCameraSpeed;
        }

        public void SetVerticalCameraSpeed(float newValue)
        {
            worldSettingsBeforeSave.verticalCameraSpeed = newValue;
            if (OnVerticalCameraSpeedChanged != null)
            {
                OnVerticalCameraSpeedChanged.Invoke(newValue);
            }
        }

        public float GetVerticalCameraSpeed()
        {
            return worldSettings.verticalCameraSpeed;
        }

        public void SaveSettings()
        {
            saveFileDataWriter = new SaveFileDataWriter()
            {
                saveDataDirectoryPath = Application.persistentDataPath
            };

            saveFileDataWriter.SavePlayerSettings(worldSettingsBeforeSave);
            worldSettings = worldSettingsBeforeSave;
        }

        public void LoadSettings()
        {
            saveFileDataWriter = new SaveFileDataWriter()
            {
                saveDataDirectoryPath = Application.persistentDataPath
            };

            saveFileDataWriter.LoadPlayerSettings(ref worldSettings);

            CheckNullValues();
        }

        private void CheckNullValues()
        {
            if (worldSettings.playerName == "")
            {
                worldSettings.playerName = "Player Name";
            }
            if (worldSettings.currentFOV == 0)
            {
                worldSettings.currentFOV = 60.0f;
            }
            if (worldSettings.horizontalCameraSpeed == 0)
            {
                worldSettings.horizontalCameraSpeed = 50;
            }
            if (worldSettings.verticalCameraSpeed == 0)
            {
                worldSettings.verticalCameraSpeed = 50;
            }
        }
    }
}