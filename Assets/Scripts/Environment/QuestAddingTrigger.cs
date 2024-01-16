using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class QuestAddingTrigger : MonoBehaviour
    {
        [SerializeField]
        private int questID = -1;

        private void OnTriggerEnter(Collider other)
        {
            //TODO : sync the quest on the network for all clients

            PlayerManager playerManager = other.GetComponent<PlayerManager>();
            if (playerManager)
            {
                Quest quest = WorldQuestManager.instance.GetQuestWithID(questID);
                if (quest != null)
                {
                    PlayerUIManager.instance.playerUIHUDManager.AddQuest(quest);
                }
            }
        }
    }
}