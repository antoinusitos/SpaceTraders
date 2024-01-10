using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

namespace AG
{
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager instance = null;

        [Header("Menus")]
        [SerializeField]
        private GameObject titleScreenMainMenu = null;
        [SerializeField]
        private GameObject titleScreenLoadMenu = null;

        [Header("Buttons")]
        [SerializeField]
        private Button mainMenuNewGameButton = null;
        [SerializeField]
        private Button loadMenuReturnButton = null;
        [SerializeField]
        private Button mainMenuLoadGameButton = null;
        [SerializeField]
        private Button deleteCharacterPopUpConfirmButton = null;

        [Header("Pop Ups")]
        [SerializeField]
        private GameObject noCharacterSlotsPopUp = null;
        [SerializeField]
        private Button noCharacterSlotsOkayButtons = null;
        [SerializeField]
        private GameObject deleteCharacterSlotPopUp = null;

        [Header("Save Slots")]
        public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;

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

        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame()
        {
            WorldSaveGameManager.instance.AttemptCreateNewGame();
        }

        public void OpenLoadGameMenu()
        {
            titleScreenMainMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);

            loadMenuReturnButton.Select();
        }

        public void CloseLoadGameMenu()
        {
            titleScreenLoadMenu.SetActive(false);
            titleScreenMainMenu.SetActive(true);

            mainMenuLoadGameButton.Select();
        }

        public void DisplayNoFreeCharacterSlotPopUp()
        {
            noCharacterSlotsPopUp.SetActive(true);
            noCharacterSlotsOkayButtons.Select();
        }

        public void CloseNoFreeCharacterSlotPopup()
        {
            noCharacterSlotsPopUp.SetActive(false);
            mainMenuNewGameButton.Select();
        }

        public void SelectCharacterSlot(CharacterSlot slot)
        {
            currentSelectedSlot = slot;
        }

        public void SelectNoSlot()
        {
            currentSelectedSlot = CharacterSlot.NO_SLOT;
        }

        public void AttemptToDeleteCharacterSlot()
        {
            if(currentSelectedSlot != CharacterSlot.NO_SLOT)
            {
                deleteCharacterSlotPopUp.SetActive(true);
                deleteCharacterPopUpConfirmButton.Select();
            }
        }

        public void DeleteCharacterSlot()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            WorldSaveGameManager.instance.DeleteGame(currentSelectedSlot);

            titleScreenLoadMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);

            loadMenuReturnButton.Select();
        }

        public void CloseDeleteCharacterPopUp()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            loadMenuReturnButton.Select();
        }
    }
}