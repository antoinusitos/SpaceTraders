using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using Unity.Netcode.Transports.UTP;

namespace AG
{
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager instance = null;

        [Header("Menus")]
        [SerializeField]
        private GameObject titleScreenLoadMenu = null;

        [Header("Buttons")]
        [SerializeField]
        private Button mainMenuNewGameButton = null;

        [Header("Save Slots")]
        public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;

        [Header("Join Menu")]
        [SerializeField]
        private InputField iPAddress = null;

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

            mainMenuNewGameButton.Select();
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
            titleScreenLoadMenu.SetActive(true);
        }

        public void CloseLoadGameMenu()
        {
            titleScreenLoadMenu.SetActive(false);
        }

        public void SelectCharacterSlot(CharacterSlot slot)
        {
            currentSelectedSlot = slot;
        }

        public void SelectNoSlot()
        {
            currentSelectedSlot = CharacterSlot.NO_SLOT;
        }

        public void JoinGame()
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = iPAddress.text;
            NetworkManager.Singleton.StartClient();
        }

        public void QuiGame()
        {
            Application.Quit();
        }
    }
}