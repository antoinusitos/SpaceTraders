using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class WorldQuestManager : MonoBehaviour
    {
        public static WorldQuestManager instance = null;

        public List<Quest> quests = new List<Quest>();

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
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public Quest GetQuestWithID(int id)
        {
            for(int i = 0; i < quests.Count; i++)
            {
                if (quests[i].questID == id)
                {
                    return quests[i];
                }
            }

            return null;
        }
    }
}