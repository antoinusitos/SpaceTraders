using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Items/Item Definition")]
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
        EQUIPPABLE,
        SCRAP,
    }
}