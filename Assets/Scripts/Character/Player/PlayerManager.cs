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
                if(playerStartsManager)
                {
                    transform.position = playerStartsManager.playersStarts[0].position;
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
                PlayerCamera.instance.player = this;
                PlayerInputManager.instance.player = this;
                WorldSaveGameManager.instance.player = this;
                PlayerFPSCamera.instance.player = this;

                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHUDManager.SetNewStaminaValue;
                playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;

                playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
                playerNetworkManager.currentStamina.Value = playerNetworkManager.maxStamina.Value;
                PlayerUIManager.instance.playerUIHUDManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);

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
            currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
            currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
            currentCharacterData.yPosition = transform.position.y;
            currentCharacterData.xPosition = transform.position.x;
            currentCharacterData.zPosition = transform.position.z;
        }

        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;
            Vector3 position = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
            transform.position = position;
        }
    }
}