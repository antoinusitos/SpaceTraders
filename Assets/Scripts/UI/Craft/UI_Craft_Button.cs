using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class UI_Craft_Button : Button
    {
        [HideInInspector]
        public PlayerUICraftManager playerUICraftManager = null;

        public int craftIDLinked = -1;
        private CraftDefinition craftLinked = null;

        protected override void Start()
        {
            base.Start();

            onClick.AddListener(Click);
        }

        public void Click()
        {
            playerUICraftManager.SetCurrentCraft(craftLinked, this);
        }

        public void RetrieveCraftLinked()
        {
            craftLinked = WorldCraftManager.instance.GetCraftWithID(craftIDLinked);
        }

        public void ShowSelected(bool newState)
        {
            GetComponent<Image>().color = newState ? Color.green : Color.white;
        }

        public void UpdateCanCraft()
        {
            if(!playerUICraftManager || !playerUICraftManager.player)
            {
                return;
            }

            if(playerUICraftManager.player.characterCraftManager.CanCraft(craftLinked))
            {
                interactable = true;
            }
            else
            {
                interactable = false;
            }
        }
    }
}