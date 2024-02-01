using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
        [HideInInspector]
        public PlayerInventoryManager playerInventoryManager = null;
        [HideInInspector]
        public PlayerEquipmentManager playerEquipmentManager = null;

        [SerializeField]
        private GameObject fpsObject = null;
        [SerializeField]
        private GameObject fpsFlashlightObject = null;
        [SerializeField]
        private GameObject tpsObject = null;
        [SerializeField]
        private GameObject tpsFlashlightObject = null;
        private Transform tpsUpDownBone = null;

        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerEffectManager = GetComponent<PlayerEffectsManager>();
            playerFPSCamera = GetComponentInChildren<PlayerFPSCamera>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();

            animator = tpsObject.GetComponent<Animator>();
            tpsUpDownBone = tpsObject.GetComponentInChildren<UpDownBone>().transform;
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

                //Invoke("LoadVisual", 0.5f);
            }
        }

        protected override void Update()
        {
            base.Update();

            if(!IsOwner)
            {
                return;
            }

            if (!isInMenu)
            {
                playerLocomotionManager.HandleAllMovement();
            }

            playerStatsManager.RegenerateStamina();
        }

        protected override void LateUpdate()
        {
            if(!IsOwner)
            {
                tpsUpDownBone.localRotation = Quaternion.Euler(playerNetworkManager.cameraUpDownAngle.Value, 0, 0);
                return;
            }

            base.LateUpdate();

            if(isDead.Value)
            {
                PlayerCamera.instance.HandleAllCameraActions();
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            playerNetworkManager.playerCharacterNumber.OnValueChanged += playerNetworkManager.OnPlayerCharacterChanged;

            if (IsOwner)
            {
                transform.position = Vector3.up;

                PlayerCamera.instance.player = this;
                PlayerInputManager.instance.player = this;
                WorldSaveGameManager.instance.player = this;
                PlayerUIManager.instance.playerUIHUDManager.player = this;
                PlayerUIManager.instance.playerUICraftManager.player = this;
                PlayerUIManager.instance.playerUICraftManager.gameObject.SetActive(false);

                playerNetworkManager.vitality.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
                playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;

                playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.instance.playerUIHUDManager.SetNewHealthValue;
                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHUDManager.SetNewStaminaValue;
                playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;

                playerNetworkManager.flashlightOn.OnValueChanged += playerInventoryManager.FlashLightState;

                tpsObject.SetActive(false);
                PlayerCamera.instance.cameraObject.gameObject.SetActive(false);

                Invoke("LoadVisual", 0.1f);
            }
            else
            {
                fpsObject.SetActive(false);
            }

            playerNetworkManager.flashlightOn.OnValueChanged += SetFlashlightUsage;
            playerNetworkManager.currentRightHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentRightHandWeaponIDChange;
        }


        private void SetFlashlightUsage(bool previousValue, bool newValue)
        {
            fpsFlashlightObject.SetActive(newValue);
            tpsFlashlightObject.SetActive(newValue);
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

        protected override void OnDeath()
        {
            base.OnDeath();

            tpsObject.SetActive(true);
            fpsObject.SetActive(false);
            PlayerUIManager.instance.playerUIHUDManager.gameObject.SetActive(false);
        }

        public void OpenCraftMenu()
        {
            if (isUsingAnInteractable)
            {
                return;
            }

            isInMenu = !isInMenu;
            PlayerUIManager.instance.playerUICraftManager.gameObject.SetActive(isInMenu);
            if(isInMenu)
            {
                PlayerUIManager.instance.playerUICraftManager.UpdateCanCraft();
            }
        }

        public void RefreshPlayerCharacterVisual(int characterNumber)
        {
            GameObject newVisual = WorldItemsManager.instance.GetCharacterWithID(characterNumber);
            if(newVisual)
            {
                GameObject instantiatedVisual = Instantiate(newVisual);
                instantiatedVisual.transform.parent = transform;
                instantiatedVisual.transform.position = tpsObject.transform.position;
                instantiatedVisual.transform.localRotation = Quaternion.identity;
                Destroy(tpsObject);
                tpsObject = instantiatedVisual;
                tpsUpDownBone = tpsObject.GetComponentInChildren<UpDownBone>().transform;
                animator = tpsObject.GetComponent<Animator>();
                playerEquipmentManager.InitializeWeaponSlot();
            }
        }

        public void LoadVisual()
        {
            Debug.Log("LoadVisual");
            playerNetworkManager.playerCharacterNumber.Value = WorldSaveGameManager.instance.currentCharacterData.characterNumber;

            PlayerManager[] players = FindObjectsOfType<PlayerManager>();
            Debug.Log("players" + players.Length);
            for(int i = 0; i < players.Length; i++)
            {
                if (players[i].IsOwner)
                    continue;

                players[i].RefreshPlayerCharacterVisual(players[i].playerNetworkManager.playerCharacterNumber.Value);
            }
        }
    }
}