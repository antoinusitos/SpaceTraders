using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class CharacterCraftManager : MonoBehaviour
    {
        private CharacterManager character = null;

        [Header("DEBUG")]
        [SerializeField]
        private int testCraftID = -1;
        [SerializeField]
        private bool craftItem = false;

        private void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        private void Update()
        {
            if (craftItem)
            {
                craftItem = false;
                CraftDefinition testCraft = WorldCraftManager.instance.GetCraftWithID(testCraftID);
                if (testCraft != null)
                {
                    TryToCraft(testCraft);
                }
            }
        }

        public bool CanCraft(CraftDefinition craft)
        {
            for (int i = 0; i < craft.neededItems.Length; i++)
            {
                if (character.characterInventoryManager.GetScrapQuantityWithID(craft.neededItems[i].itemID) < craft.neededItems[i].itemQuantity)
                {
                    return false;
                }
            }
            return true;
        }

        public void TryToCraft(CraftDefinition craft)
        {
            for(int i = 0; i < craft.neededItems.Length; i++)
            {
                if (character.characterInventoryManager.GetScrapQuantityWithID(craft.neededItems[i].itemID) < craft.neededItems[i].itemQuantity)
                {
                    return;
                }
            }

            CraftItem(craft);
        }

        protected void CraftItem(CraftDefinition craft)
        {
            for (int i = 0; i < craft.neededItems.Length; i++)
            {
                character.characterInventoryManager.RemoveScrapWithID(craft.neededItems[i].itemID, craft.neededItems[i].itemQuantity);
            }

            character.characterInventoryManager.AddItem(WorldItemsManager.instance.GetItemWithID(craft.createdItemID));
        }
    }
}