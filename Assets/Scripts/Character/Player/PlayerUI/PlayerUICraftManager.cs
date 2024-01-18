using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class PlayerUICraftManager : MonoBehaviour
    {
        [HideInInspector]
        public PlayerManager player = null;

        [SerializeField]
        private UI_Craft_Activator[] activators = null;
        [SerializeField]
        private UI_Craft_Button[] craftButtons = null;
        private UI_Craft_Button lastCraftButton = null;

        private CraftDefinition currentCraft = null;

        [SerializeField]
        private Button craftValidation = null;

        public void Start()
        {
            for(int i = 0; i < activators.Length; i++)
            {
                activators[i].playerUICraftManager = this;
            }

            ResetActivators();

            for (int i = 0; i < craftButtons.Length; i++)
            {
                craftButtons[i].playerUICraftManager = this;
                craftButtons[i].RetrieveCraftLinked();
            }

            craftValidation.onClick.AddListener(ValidateCraft);
        }

        public void OnActivatorTriggered()
        {
            for (int i = 0; i < activators.Length; i++)
            {
                if(!activators[i].isActivated)
                {
                    return;
                }
            }

            craftValidation.interactable = true;
        }

        private void ValidateCraft()
        {
            player.characterCraftManager.TryToCraft(currentCraft);
            ResetActivators();
            currentCraft = null;
            if (lastCraftButton)
            {
                lastCraftButton.ShowSelected(false);
                lastCraftButton.UpdateCanCraft();
                lastCraftButton = null;
            }
        }

        public void ResetActivators()
        {
            for (int i = 0; i < activators.Length; i++)
            {
                activators[i].ResetActivator();
                activators[i].interactable = false;
            }

            craftValidation.interactable = false;
        }

        public void EnableActivators()
        {
            for (int i = 0; i < activators.Length; i++)
            {
                activators[i].interactable = true;
            }
        }

        public void SetCurrentCraft(CraftDefinition craft, UI_Craft_Button newCraftButton)
        {
            if(craft != currentCraft)
            {
                if(lastCraftButton)
                {
                    lastCraftButton.ShowSelected(false);
                }
                lastCraftButton = newCraftButton;
                lastCraftButton.ShowSelected(true);

                currentCraft = craft;
                ResetActivators();
                EnableActivators();
            }
        }

        public void UpdateCanCraft()
        {
            for (int i = 0; i < craftButtons.Length; i++)
            {
                craftButtons[i].UpdateCanCraft();
            }
        }
    }
}