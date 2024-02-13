using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace AG
{
    public class CharacterInventoryManager : MonoBehaviour
    {
        private CharacterManager character = null;

        public ItemDefinition[] inventory = null;
        protected int maxInventorySize = 3;
        public List<ItemDefinition> scrapInventory = new List<ItemDefinition>();

        protected int flashlightBatteriesNumber = 0;

        [Header("DEBUG")]
        [SerializeField]
        private int testItemID = -1;
        [SerializeField]
        private bool giveItem = false;
        [SerializeField]
        private bool useItem = false;

        public WeaponItem currentRightHandWeapon = null;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            inventory = new ItemDefinition[3];
        }

        protected virtual void Update()
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
                UseItemWithID(0);
            }
        }

        public bool AddItem(ItemDefinition item)
        {
            if(item.itemType == ItemType.SCRAP)
            {
                return AffectInScrapInventory(item);
            }
            else
            {
                return AffectInInventory(item);
            }
        }

        private bool AffectInInventory(ItemDefinition item)
        {
            if(item.itemType == ItemType.FLASHLIGHTBATTERY)
            {
                flashlightBatteriesNumber++;
                PlayerUIManager.instance.playerUIHUDManager.UpdateFlashLightBatteriesNumber(flashlightBatteriesNumber);
                return true;
            }

            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == null)
                {
                    inventory[i] = item;
                    if(i == character.characterEquipmentManager.GetCurrentSlotUsed())
                    {
                        UseItemWithID(i);
                    }
                    PlayerUIManager.instance.playerUIInventoryManager.AffectSpriteToItem(item.itemSprite, i);
                    return true;
                }
            }

            return false;
        }

        private bool AffectInScrapInventory(ItemDefinition item)
        {
            for(int i = 0; i < scrapInventory.Count; i++)
            {
                if (scrapInventory[i].itemID == item.itemID)
                {
                    scrapInventory[i].itemQuantity += item.itemQuantity;
                    return true;
                }
            }
            scrapInventory.Add(Instantiate(item));
            return true;
        }

        public bool AddItem(Item item)
        {
            return AddItem(item.itemDefinition);
        }

        public void UseItemWithID(int itemIndex)
        {
            if(inventory[itemIndex] == null)
            {
                return;
            }

            if (inventory[itemIndex].itemType == ItemType.CONSUMABLE)
            {
                GameObject spawnedPrefab = Instantiate(inventory[itemIndex].itemPrefab);
                if (spawnedPrefab)
                {
                    Item item = spawnedPrefab.GetComponent<Item>();
                    if (item)
                    {
                        item.itemOwner = character;
                        item.itemDefinition = inventory[itemIndex];
                        item.UseItem();
                        PlayerUIManager.instance.playerUIInventoryManager.EmptySlot(itemIndex);
                        inventory[itemIndex] = null;
                        Destroy(spawnedPrefab);
                    }
                }
            }
            else
            {
                if (inventory[itemIndex].itemType == ItemType.EQUIPPABLE)
                {
                    ((PlayerManager)character).playerNetworkManager.currentRightHandWeaponID.Value = inventory[itemIndex].itemID;
                    character.characterEquipmentManager.SetCurrentSlotUsed(itemIndex);
                    PlayerUIManager.instance.playerUIInventoryManager.SetUsedSlot(itemIndex);
                }
            }
        }

        public void RemoveItemWithID(int itemID)
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i].itemID == itemID)
                {
                    inventory[i] = null;
                    return;
                }
            }
        }

        public void RemoveItemFromSlot(int slot)
        {
            inventory[slot] = null;
        }

        public void RemoveScrapWithID(int itemID, int quantity)
        {
            for (int i = 0; i < scrapInventory.Count; i++)
            {
                if (scrapInventory[i].itemID == itemID)
                {
                    scrapInventory[i].itemQuantity -= quantity;
                    if (scrapInventory[i].itemQuantity <= 0)
                    {
                        scrapInventory.RemoveAt(i);
                    }
                    return;
                }
            }
        }

        public int GetScrapQuantityWithID(int itemID)
        {
            for (int i = 0; i < scrapInventory.Count; i++)
            {
                if (scrapInventory[i].itemID == itemID)
                {
                    return scrapInventory[i].itemQuantity;
                }
            }
            return 0;
        }

        public void TryToUseItem(int index)
        {
            PlayerUIManager.instance.playerUIInventoryManager.SetUsedSlot(index);
            if (inventory[index] == null)
            {
                character.characterNetworkManager.currentWeaponBeingUsed.Value = -1;
                character.characterNetworkManager.currentRightHandWeaponID.Value = -1;
                character.characterInventoryManager.currentRightHandWeapon = null;
                character.characterEquipmentManager.LoadRightWeapon();
                return;
            }

            UseItemWithID(index);
        }
    }
}