using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class WorldCraftManager : MonoBehaviour
    {
        public static WorldCraftManager instance = null;

        public List<CraftDefinition> allCrafts;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            for (int i = 0; i < allCrafts.Count; i++)
            {
                allCrafts[i].craftID = i;
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public CraftDefinition GetCraftWithID(int id)
        {
            for (int i = 0; i < allCrafts.Count; i++)
            {
                if (allCrafts[i].craftID == id)
                {
                    return allCrafts[i];
                }
            }
            return null;
        }
    }
}