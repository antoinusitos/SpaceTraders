using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AG
{
    public class PlayerUIHUDManager : MonoBehaviour
    {
        [HideInInspector]
        public PlayerManager player = null;

        [SerializeField]
        private UI_StatBar staminaBar = null;
        [SerializeField]
        private UI_StatBar healthBar = null;

        [SerializeField]
        private Transform questGroup = null;
        [SerializeField]
        private GameObject questTextPrefab = null;
        private List<UI_QuestText> QuestTexts = new List<UI_QuestText>();

        public void SetNewHealthValue(float oldValue, float newValue)
        {
            healthBar.SetStat(newValue);
        }

        public void SetMaxHealthValue(int maxHealth)
        {
            healthBar.SetMaxStat(maxHealth);
        }

        public void SetNewStaminaValue(float oldValue, float newValue)
        {
            staminaBar.SetStat(newValue);
            if (player.characterNetworkManager.currentStamina.Value >= player.characterNetworkManager.maxStamina.Value)
            {
                staminaBar.gameObject.SetActive(false);
            }
            else
            {
                staminaBar.gameObject.SetActive(true);
            }
        }

        public void SetMaxStaminaValue(int maxStamina)
        {
            staminaBar.SetMaxStat(maxStamina);
        }

        public void RefreshHUD()
        {
            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);
            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);
        }

        public void AddQuest(Quest newQuest)
        {
            for (int i = 0; i < QuestTexts.Count; i++)
            {
                if (QuestTexts[i].questID == newQuest.questID)
                {
                    return;
                }
            }

            GameObject spawnedQuestObject = Instantiate(questTextPrefab);
            UI_QuestText uI_QuestText = new UI_QuestText()
            {
                questID = newQuest.questID,
                textGameObject = spawnedQuestObject.GetComponent<TextMeshProUGUI>()
            };
            uI_QuestText.textGameObject.text = newQuest.questDesc;
            QuestTexts.Add(uI_QuestText);
            spawnedQuestObject.transform.SetParent(questGroup);
        }

        public void RemoveQuest(int questID)
        {
            for(int i = 0; i < QuestTexts.Count; i++)
            {
                if (QuestTexts[i].questID == questID)
                {
                    Destroy(QuestTexts[i].textGameObject.gameObject);
                    QuestTexts.RemoveAt(i);
                    return;
                }
            }
        }
    }
}