using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class WorldItemsManager : MonoBehaviour
    {
        public static WorldItemsManager instance = null;

        public List<ItemDefinition> allItems;

        public List<PickableItem> allPickableItems;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            for (int i = 0; i < allItems.Count; i++)
            {
                allItems[i].itemID = i;
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public ItemDefinition GetItemWithID(int id)
        {
            for(int i = 0; i < allItems.Count; i++)
            {
                if (allItems[i].itemID == id)
                {
                    return allItems[i];
                }
            }
            return null;
        }

        public PickableItem GetRandomPickableItem()
        {
            return allPickableItems[Random.Range(0, allPickableItems.Count)];
        }
    }
}