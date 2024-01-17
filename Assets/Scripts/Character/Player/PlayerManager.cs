using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AG
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector]
        public PlayerAnimatorManager playerAnimatorManager = null;
        [HideInInspector]
        public PlayerLocomotionManager playerLocomotionManager = null;
        [HideInInspector]
        public PlayerNetworkManager playerNetworkManager = null;
        [HideInInspector]
        public PlayerStatsManager playerStatsManager = null;
        [HideInInspector]
        public PlayerEffectsManager playerEffectManager = null;
        [HideInInspector]
        public PlayerFPSCamera playerFPSCamera = null;

        [SerializeField]
        private GameObject fpsObject = null;
        [SerializeField]
        private GameObject tpsObject = null;

        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerEffectManager = GetComponent<PlayerEffectsManager>();
            playerFPSCamera = GetComponentInChildren<PlayerFPSCamera>();
        }

        private void Start()
        {
            SceneManager.activeSceneChanged += OnSceneChange;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (!IsOwner)
            {
                return;
            }

            if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
            {
                WorldPlayerStartsManager playerStartsManager = FindObjectOfType<WorldPlayerStartsManager>();
                if (playerStartsManager)
                {
                    characterController.enabled = false;
                    transform.position = playerStartsManager.playersStarts[2].position;
                    characterController.enabled = true;
                }
            }
        }

        protected override void Update()
        {
            base.Update();

            if(!IsOwner)
            {
                return;
            }

            playerLocomotionManager.HandleAllMovement();

            playerStatsManager.RegenerateStamina();
        }

        protected override void LateUpdate()
        {
            if(!IsOwner)
            {
                return;
            }

            base.LateUpdate();

            //PlayerCamera.instance.HandleAllCameraActions();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if(IsOwner)
            {
                transform.position = Vector3.up;

                PlayerCamera.instance.player = this;
                PlayerInputManager.instance.player = this;
                WorldSaveGameManager.instance.player = this;
                PlayerUIManager.instance.playerUIHUDManager.player = this;

                playerNetworkManager.vitality.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
                playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;

                playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.instance.playerUIHUDManager.SetNewHealthValue;
                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHUDManager.SetNewStaminaValue;
                playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;

                tpsObject.SetActive(false);
                PlayerCamera.instance.gameObject.SetActive(false);
            }
            else
            {
                fpsObject.SetActive(false);
            }
        }

        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            //currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
            currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
            //currentCharacterData.yPosition = transform.position.y;
            //currentCharacterData.xPosition = transform.position.x;
            //currentCharacterData.zPosition = transform.position.z;
            currentCharacterData.endurance = playerNetworkManager.endurance.Value;
            currentCharacterData.vitality = playerNetworkManager.vitality.Value;
        }

        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;
            //Vector3 position = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
            //transform.position = position;

            playerNetworkManager.vitality.Value = currentCharacterData.vitality;
            playerNetworkManager.endurance.Value = currentCharacterData.endurance;

            playerNetworkManager.maxHealth.Value = playerStatsManager.CalculateHealthBasedOnVitalityLevel(playerNetworkManager.vitality.Value);
            playerNetworkManager.currentHealth.Value = playerNetworkManager.maxHealth.Value;
            PlayerUIManager.instance.playerUIHUDManager.SetMaxHealthValue(playerNetworkManager.maxHealth.Value);
            playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
            playerNetworkManager.currentStamina.Value = playerNetworkManager.maxStamina.Value;
            PlayerUIManager.instance.playerUIHUDManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
        }

        public void AddQuest(int questID)
        {
            Quest quest = WorldQuestManager.instance.GetQuestWithID(questID);
            if (quest != null)
            {
                PlayerUIManager.instance.playerUIHUDManager.AddQuest(quest);
            }
        }
    }
}