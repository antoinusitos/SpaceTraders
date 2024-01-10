using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class TitleScreenLoadMenuInputManager : MonoBehaviour
    {
        private PlayerControls playerControls = null;

        [Header("Title Screen Inputs")]
        [SerializeField]
        private bool deleteCharacterSlot = false;

        private void Update()
        {
            if(deleteCharacterSlot)
            {
                deleteCharacterSlot = false;
                TitleScreenManager.instance.AttemptToDeleteCharacterSlot();
            }
        }

        private void OnEnable()
        {
            if(playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.UI.X.performed += _ => deleteCharacterSlot = true;
            }

            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }
    }
}