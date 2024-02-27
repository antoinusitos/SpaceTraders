using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace AG
{
    public class SaveFileDataWriter
    {
        public string saveDataDirectoryPath = "Character";
        public string saveFileName = "";

        public string saveSettingsDirectoryPath = "Settings";
        public string saveSettingsFileName = "PlayerSettings";

        public bool CheckToSeeIfFileExists()
        {
            if(File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
        }

        public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
        {
            string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("CREATING SAVE FILE, AT SAVE PATH : " + savePath);

                string dataToStore = JsonUtility.ToJson(characterData, true);

                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter fileWriter = new StreamWriter(stream))
                    {
                        fileWriter.Write(dataToStore);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("ERROR WHILST TRYING TO SAVE CHARACTER DATA, GAME NOT SAVED at " + savePath + "\n" + ex);
            }
        }

        public CharacterSaveData LoadSaveFile()
        {
            CharacterSaveData characterData = null;

            string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

            if(File.Exists(loadPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
                }
                catch (Exception ex)
                {
                    Debug.LogError("ERROR WHILST TRYING TO LOAD CHARACTER DATA, GAME NOT LOADED at " + loadPath + "\n" + ex);
                }
            }

            return characterData;
        }

        public void SavePlayerSettings(WorldSettings settings)
        {
            string savePath = Path.Combine(saveSettingsDirectoryPath, saveSettingsFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("CREATING SAVE FILE, AT SAVE PATH : " + savePath);

                string dataToStore = JsonUtility.ToJson(settings, true);

                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter fileWriter = new StreamWriter(stream))
                    {
                        fileWriter.Write(dataToStore);
                    }
                }

                Debug.Log("PLAYER SETTINGS SAVED at " + savePath);
            }
            catch (Exception ex)
            {
                Debug.LogError("ERROR WHILST TRYING TO SAVE PLAYER SETTINGS, SETTINGS NOT SAVED at " + savePath + "\n" + ex);
            }
        }

        public void LoadPlayerSettings(ref WorldSettings settings)
        {
            string loadPath = Path.Combine(saveSettingsDirectoryPath, saveSettingsFileName);

            if (File.Exists(loadPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    settings = JsonUtility.FromJson<WorldSettings>(dataToLoad);

                    Debug.Log("PLAYER SETTINGS LOADED");
                }
                catch (Exception ex)
                {
                    Debug.LogError("ERROR WHILST TRYING TO LOAD PLAYER SETTINGS, SETTINGS NOT LOADED at " + loadPath + "\n" + ex);
                }
            }
        }
    }
}