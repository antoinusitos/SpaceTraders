using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AG
{
    public class UI_Character_Save_Slot : MonoBehaviour
    {
        private SaveFileDataWriter saveFileDataWriter = null;

        [Header("Game Slot")]
        public CharacterSlot characterSlot;

        [Header("Character Info")]
        public TextMeshProUGUI characterName = null;
        public TextMeshProUGUI timePlayed = null;

        private void OnEnable()
        {
            LoadSaveSlots();
        }

        private void LoadSaveSlots()
        {
            saveFileDataWriter = new SaveFileDataWriter()
            {
                saveDataDirectoryPath = Application.persistentDataPath
            };

            saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeinUsed(characterSlot);

            switch (characterSlot)
            {
                case CharacterSlot.CharacterSlot_01:
                    if (saveFileDataWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case CharacterSlot.CharacterSlot_02:
                    if (saveFileDataWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot02.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case CharacterSlot.CharacterSlot_03:
                    if (saveFileDataWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot03.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case CharacterSlot.CharacterSlot_04:
                    if (saveFileDataWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot04.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case CharacterSlot.CharacterSlot_05:
                    if (saveFileDataWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot05.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case CharacterSlot.CharacterSlot_06:
                    if (saveFileDataWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot06.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case CharacterSlot.CharacterSlot_07:
                    if (saveFileDataWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot07.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case CharacterSlot.CharacterSlot_08:
                    if (saveFileDataWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot08.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case CharacterSlot.CharacterSlot_09:
                    if (saveFileDataWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot09.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case CharacterSlot.CharacterSlot_10:
                    if (saveFileDataWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot10.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
            }
        }

        public void LoadGameFromCharacterSlot()
        {
            WorldSaveGameManager.instance.currentCharacterSlotBeingUsed = characterSlot;
            WorldSaveGameManager.instance.LoadGame();
        }

        public void SelectCurrentSlot()
        {
            TitleScreenManager.instance.SelectCharacterSlot(characterSlot);
        }
    }
}