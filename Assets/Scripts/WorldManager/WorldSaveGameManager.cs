using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AG
{
    public class WorldSaveGameManager : NetworkBehaviour
    {
        public static WorldSaveGameManager instance = null;

#if UNITY_EDITOR
        public UnityEditor.SceneAsset SceneAsset;
        private void OnValidate()
        {
            if (SceneAsset != null)
            {
                m_SceneName = SceneAsset.name;
            }
        }
#endif
        [SerializeField]
        private string m_SceneName;

        public PlayerManager player = null;

        [Header("Save/Load")]
        [SerializeField]
        private bool saveGame = false;
        [SerializeField]
        private bool loadGame = false;

        [Header("World Scene Index")]
        private int worldSceneIndex = 2;

        [Header("Save Data Writer")]
        private SaveFileDataWriter saveFileDataWriter = null;

        [Header("Current Character Data")]
        public CharacterSlot currentCharacterSlotBeingUsed;
        public CharacterSaveData currentCharacterData = null;
        private string saveFileName = "";

        [Header("Character Slots")]
        public CharacterSaveData characterSlot01 = null;
        public CharacterSaveData characterSlot02 = null;
        public CharacterSaveData characterSlot03 = null;
        public CharacterSaveData characterSlot04 = null;
        public CharacterSaveData characterSlot05 = null;
        public CharacterSaveData characterSlot06 = null;
        public CharacterSaveData characterSlot07 = null;
        public CharacterSaveData characterSlot08 = null;
        public CharacterSaveData characterSlot09 = null;
        public CharacterSaveData characterSlot10 = null;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            LoadAllCharacterProfiles();
        }

        private void Update()
        {
            if(saveGame)
            {
                saveGame = false;
                SaveGame();
            }

            if(loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }

        public string DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(CharacterSlot characterSlot)
        {
            string fileName = "";
            switch(characterSlot) 
            {
                case CharacterSlot.CharacterSlot_01:
                    fileName = "CharacterSlot_01";
                    break;
                case CharacterSlot.CharacterSlot_02:
                    fileName = "CharacterSlot_02";
                    break;
                case CharacterSlot.CharacterSlot_03:
                    fileName = "CharacterSlot_03";
                    break;
                case CharacterSlot.CharacterSlot_04:
                    fileName = "CharacterSlot_04";
                    break;
                case CharacterSlot.CharacterSlot_05:
                    fileName = "CharacterSlot_05";
                    break;
                case CharacterSlot.CharacterSlot_06:
                    fileName = "CharacterSlot_06";
                    break;
                case CharacterSlot.CharacterSlot_07:
                    fileName = "CharacterSlot_07";
                    break;
                case CharacterSlot.CharacterSlot_08:
                    fileName = "CharacterSlot_08";
                    break;
                case CharacterSlot.CharacterSlot_09:
                    fileName = "CharacterSlot_09";
                    break;
                case CharacterSlot.CharacterSlot_10:
                    fileName = "CharacterSlot_10";
                    break;
            }

            return fileName;
        }

        public void AttemptCreateNewGame()
        {
            saveFileDataWriter = new SaveFileDataWriter
            {
                saveDataDirectoryPath = Application.persistentDataPath,
            };

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(CharacterSlot.CharacterSlot_01);

            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01;
            currentCharacterData = new CharacterSaveData();
            NewGame();

            /*saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(CharacterSlot.CharacterSlot_02);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_02;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(CharacterSlot.CharacterSlot_03);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_03;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(CharacterSlot.CharacterSlot_04);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_04;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(CharacterSlot.CharacterSlot_05);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_05;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(CharacterSlot.CharacterSlot_06);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_06;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(CharacterSlot.CharacterSlot_07);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_07;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(CharacterSlot.CharacterSlot_08);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_08;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(CharacterSlot.CharacterSlot_09);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_09;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(CharacterSlot.CharacterSlot_10);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_10;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }

            TitleScreenManager.instance.DisplayNoFreeCharacterSlotPopUp();*/
        }

        private void NewGame()
        {
            player.characterNetworkManager.vitality.Value = 10;
            player.characterNetworkManager.endurance.Value = 10;

            SaveGame();
            //StartCoroutine(LoadWorldScene());
            LoadWorldScene();
        }

        public void LoadGame()
        {
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(currentCharacterSlotBeingUsed);

            saveFileDataWriter = new SaveFileDataWriter
            {
                saveDataDirectoryPath = Application.persistentDataPath,
                saveFileName = saveFileName
            };

            currentCharacterData = saveFileDataWriter.LoadSaveFile();
            currentCharacterData.characterNumber = 0;

            //StartCoroutine(LoadWorldScene());
            LoadWorldScene();
        }

        public void SaveGame()
        {
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(currentCharacterSlotBeingUsed);

            saveFileDataWriter = new SaveFileDataWriter()
            {
                saveDataDirectoryPath = Application.persistentDataPath,
                saveFileName = saveFileName
            };

            player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);

            saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
        }

        public void DeleteGame(CharacterSlot characterSlot)
        {

            saveFileDataWriter = new SaveFileDataWriter()
            {
                saveDataDirectoryPath = Application.persistentDataPath,
                saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(characterSlot)
        };
            saveFileDataWriter.DeleteSaveFile();
        }

        public void LoadAllCharacterProfiles()
        {
            saveFileDataWriter = new SaveFileDataWriter
            {
                saveDataDirectoryPath = Application.persistentDataPath
            };

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(CharacterSlot.CharacterSlot_01);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(CharacterSlot.CharacterSlot_02);
            characterSlot02 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(CharacterSlot.CharacterSlot_03);
            characterSlot03 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(CharacterSlot.CharacterSlot_04);
            characterSlot04 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(CharacterSlot.CharacterSlot_05);
            characterSlot05 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(CharacterSlot.CharacterSlot_06);
            characterSlot06 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(CharacterSlot.CharacterSlot_07);
            characterSlot07 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(CharacterSlot.CharacterSlot_08);
            characterSlot08 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(CharacterSlot.CharacterSlot_09);
            characterSlot09 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(CharacterSlot.CharacterSlot_10);
            characterSlot10 = saveFileDataWriter.LoadSaveFile();

        }

        /*public IEnumerator LoadWorldScene()
        {
            NetworkManager.SceneManager.LoadScene("HUB", LoadSceneMode.Single);
            player.LoadGameDataFromCurrentCharacterData(ref currentCharacterData);

            yield return null;
        }*/

        public void LoadWorldScene()
        {
            player.LoadGameDataFromCurrentCharacterData(ref currentCharacterData);
            NetworkManager.SceneManager.LoadScene(m_SceneName, LoadSceneMode.Single);

            //yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}