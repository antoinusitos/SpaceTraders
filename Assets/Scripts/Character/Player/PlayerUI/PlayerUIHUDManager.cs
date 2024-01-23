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

        [Header("Bars")]
        [SerializeField]
        private UI_StatBar staminaBar = null;
        [SerializeField]
        private UI_StatBar healthBar = null;

        [Header("Quests")]
        [SerializeField]
        private Transform questGroup = null;
        [SerializeField]
        private GameObject questTextPrefab = null;
        private List<UI_QuestText> QuestTexts = new List<UI_QuestText>();

        [Header("Factions")]
        [SerializeField]
        private TextMeshProUGUI factiontext = null;

        [Header("Game State")]
        [SerializeField]
        private GameObject waitForPlayersText = null;
        [SerializeField]
        private TextMeshProUGUI seedText = null;
        private float seedShowingDuration = 4;

        [Header("Flashlight")]
        [SerializeField]
        private TextMeshProUGUI flashLightBatteriesNumberText = null;
        [SerializeField]
        private UI_StatBar flashlightBatterieBar = null;
        [SerializeField]
        private UI_StatBar flashlightRefillBar = null;

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

        public void ShowFaction(Factions faction)
        {
            factiontext.gameObject.SetActive(faction != Factions.NONE);
            factiontext.text = faction.ToString();
        }

        public void ShowWaitingPlayers(bool mustShow)
        {
            waitForPlayersText.SetActive(mustShow);
        }

        public void UpdateFlashLightBatteriesNumber(int newValue)
        {
            flashLightBatteriesNumberText.text = newValue.ToString();
        }

        public void UpdateFlashLightBatterie(float currentValue, float maxValue)
        {
            flashlightBatterieBar.SetMaxStat(Mathf.RoundToInt(maxValue));
            flashlightBatterieBar.SetStat(currentValue);
        }

        public void UpdateRefillSlider(float currentValueRatio)
        {
            if(currentValueRatio > 0)
            {
                flashlightRefillBar.gameObject.SetActive(true);
            }
            else
            {
                flashlightRefillBar.gameObject.SetActive(false);
            }
            flashlightRefillBar.SetMaxStat(1);
            flashlightRefillBar.SetStat(currentValueRatio);
        }

        public void ShowSeed(int seed)
        {
            seedText.text = "Seed : " + seed.ToString();
            seedText.gameObject.SetActive(true);
            Invoke("HideSeed", seedShowingDuration);
        }

        private void HideSeed()
        {
            seedText.gameObject.SetActive(false);
        }
    }
}