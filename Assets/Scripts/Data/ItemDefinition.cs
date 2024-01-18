using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Items/Consumable/Heal Consumable")]
    public class ItemDefinition : ScriptableObject
    {
        public int itemID = -1;
        public string itemName = "";
        public ItemType itemType = ItemType.NONE;
        public int itemQuantity = 0;
        public GameObject itemPrefab = null;
    }

    public enum ItemType
    {
        NONE,
        CONSUMABLE,
        WEAPON,
        SCRAP
    }
}