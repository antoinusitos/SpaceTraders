using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PlayerUIPauseManager : MonoBehaviour
    {
        [HideInInspector]
        public PlayerManager player = null;

        [SerializeField]
        private GameObject pauseMenuContent = null;

        public void ShowPauseMenu()
        {
            pauseMenuContent.SetActive(true);
        }

        public void HidePauseMenu()
        {
            pauseMenuContent.SetActive(false);
            player.currentFocusType = FocusType.Game;
        }

        public void QuitGame()
        {
            HidePauseMenu();
            player.QuitGame();
        }
    }
}