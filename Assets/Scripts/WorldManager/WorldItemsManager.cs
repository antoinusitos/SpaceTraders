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

        public List<GameObject> allCharacters;

        public List<WeaponItem> allWeapons;

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

            for (int i = 0; i < allWeapons.Count; i++)
            {
                allItems.Add(allWeapons[i]);
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

        public GameObject GetCharacterWithID(int id)
        {
            if (id < 0 || id >= allCharacters.Count)
            {
                return null;
            }

            return allCharacters[id];
        }

        public WeaponItem GetWeaponWithID(int id)
        {
            if (id < 0 || id >= allWeapons.Count)
            {
                return null;
            }

            for (int i = 0; i < allWeapons.Count; i++)
            {
                if (allWeapons[i].itemID == id)
                {
                    return allWeapons[i];
                }
            }

            return null;
        }
    }
}