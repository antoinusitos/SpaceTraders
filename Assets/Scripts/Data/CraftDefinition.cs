using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Crafts/Craft Definition")]
    public class CraftDefinition : ScriptableObject
    {
        public int craftID = -1;

        public int createdItemID = -1;

        public CraftRequirement[] neededItems = null;
    }

    [System.Serializable]
    public class CraftRequirement
    {
        public int itemID = -1;
        public int itemQuantity = -1;
    }
}