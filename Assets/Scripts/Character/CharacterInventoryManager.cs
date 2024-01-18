using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class CharacterInventoryManager : MonoBehaviour
    {
        private CharacterManager character = null;

        public List<ItemDefinition> inventory = new List<ItemDefinition>();

        [Header("DEBUG")]
        [SerializeField]
        private int testItemID = -1;
        [SerializeField]
        private bool giveItem = false;
        [SerializeField]
        private bool useItem = false;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        private void Update()
        {
            if (giveItem)
            {
                giveItem = false;
                ItemDefinition testItem = WorldItemsManager.instance.GetItemWithID(testItemID);
                if (testItem != null)
                {
                    AddItem(testItem);
                }
            }
            if (useItem)
            {
                useItem = false;
                UseItemWithID(testItemID);
            }
        }

        public void AddItem(ItemDefinition item)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].itemID == item.itemID)
                {
                    inventory[i].itemQuantity += item.itemQuantity;
                    return;
                }
            }

            inventory.Add(Instantiate(item));
        }

        public void AddItem(Item item)
        {
            AddItem(item.itemDefinition);
        }

        public void UseItemWithID(int itemID)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].itemID == itemID)
                {
                    if (inventory[i].itemType == ItemType.CONSUMABLE)
                    {
                        GameObject spawnedPrefab = Instantiate(inventory[i].itemPrefab);
                        if (spawnedPrefab)
                        {
                            Item item = spawnedPrefab.GetComponent<Item>();
                            if (item)
                            {
                                item.itemOwner = character;
                                item.itemDefinition = inventory[i];
                                item.UseItem();
                                inventory[i].itemQuantity--;
                                if (inventory[i].itemQuantity <= 0)
                                {
                                    inventory.RemoveAt(i);
                                }
                                Destroy(spawnedPrefab);
                            }
                        }
                    }
                    return;
                }
            }
        }

        public void RemoveItemWithID(int itemID, int quantity)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].itemID == itemID)
                {
                    inventory[i].itemQuantity -= quantity;
                    if (inventory[i].itemQuantity <= 0)
                    {
                        inventory.RemoveAt(i);
                    }
                    return;
                }
            }
        }

        public int GetItemQuantityWithID(int itemID)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].itemID == itemID)
                {
                    return inventory[i].itemQuantity;
                }
            }
            return 0;
        }
    }
}