using UnityEngine;
using UnityEngine.SceneManagement;

namespace AG
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance = null;

        public PlayerManager player = null;

        private PlayerControls playerControls = null;

        [Header("PLAYER MOVEMENT INPUT")]
        [SerializeField]
        private Vector2 movementInput = Vector2.zero;
        public float horizontalInput = 0.0f;
        public float verticalInput = 0.0f;
        public float moveAmount = 0.0f;

        [Header("CAMERA MOVEMENT INPUT")]
        [SerializeField]
        private Vector2 cameraInput = Vector2.zero;
        public float cameraHorizontalInput = 0.0f;
        public float cameraVerticalInput = 0.0f;

        [Header("PLAYER ACTION INPUT")]
        [SerializeField]
        private bool sprintInput = false;
        [SerializeField]
        private bool crouchInput = false;
        [SerializeField]
        private bool jumpInput = false;
        [SerializeField]
        private bool craftMenuInput = false;
        [SerializeField]
        private bool interactionInput = false;
        [SerializeField]
        private bool quickUsed1Input = false;
        [SerializeField]
        private bool quickUsed2Input = false;
        [SerializeField]
        private bool quickUsed3Input = false;
        [SerializeField]
        private bool flashlightInput = false;
        [SerializeField]
        private bool refillFlashlightInput = false;
        [SerializeField]
        private bool useItemInput = false;
        [SerializeField]
        private bool pauseMenuInput = false;

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
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if(newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;

                if (playerControls != null)
                {
                    playerControls.Enable();
                }
            }
            else
            {
                instance.enabled = false;

                if (playerControls != null)
                {
                    playerControls.Disable();
                }
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            SceneManager.activeSceneChanged += OnSceneChange;

            instance.enabled = false;
            if(playerControls != null)
            {
                playerControls.Disable();
            }
        }

        private void OnEnable()
        {
            if(playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
                playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
                playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
                playerControls.PlayerActions.Crouch.performed += i => crouchInput = true;
                //playerControls.PlayerActions.Crouch.canceled += i => crouchInput = false;
                playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
                playerControls.PlayerActions.CraftMenu.performed += i => craftMenuInput = true;
                playerControls.PlayerActions.Interaction.performed += i => interactionInput = true;
                playerControls.PlayerActions.QuickUse1.performed += i => quickUsed1Input = true;
                playerControls.PlayerActions.QuickUse2.performed += i => quickUsed2Input = true;
                playerControls.PlayerActions.QuickUse3.performed += i => quickUsed3Input = true;
                playerControls.PlayerActions.Flashlight.performed += i => flashlightInput = true;
                playerControls.PlayerActions.RefillFlashLight.performed += i => refillFlashlightInput = true;
                playerControls.PlayerActions.RefillFlashLight.canceled += i => refillFlashlightInput = false;
                playerControls.PlayerActions.UseItem.performed += i => useItemInput = true;
                playerControls.PlayerActions.PauseMenu.performed += i => pauseMenuInput = true;

            }

            playerControls.Enable();
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        private void OnApplicationFocus(bool focus)
        {
            if(enabled)
            {
                if(focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }

        private void Update()
        {
            HandleAllInputs();
        }

        private void HandleAllInputs()
        {
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
            HandleSprintInput();
            HandleCrouchInput();
            HandleJumpInput();
            HandleCraftMenuInput();
            HandleInteractionInput();
            HandleQuickUseInput();
            HandleFlashLightInput();
            HandleRefillFlashLightInput();
            HandleUseItemInput();
            HandlePauseMenuInput();
        }

        private void HandlePlayerMovementInput()
        {
            if(player.currentFocusType != FocusType.Game)
            {
                verticalInput = 0;
                horizontalInput = 0;
                return;
            }

            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            if(moveAmount <= 0.5f && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if(moveAmount > 0.5f && moveAmount <= 1)
            {
                moveAmount = 1;
            }

            if(player == null)
            {
                return;
            }

            player.playerAnimatorManager.UpdateAnimatorValuesParamaters(horizontalInput, verticalInput, player.playerNetworkManager.isSprinting.Value);
        }

        private void HandleCameraMovementInput()
        {
            if (player.currentFocusType != FocusType.Game)
            {
                cameraVerticalInput = 0;
                cameraHorizontalInput = 0;
                return;
            }

            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }

        private void HandleSprintInput()
        {
            if (sprintInput && player.currentFocusType == FocusType.Game)
            {
                player.playerLocomotionManager.HandleSprinting();
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
        }

        private void HandleCrouchInput()
        {
            if(!player)
            {
                return;
            }

            if (player.currentFocusType != FocusType.Game)
            {
                return;
            }

            if (crouchInput)
            {
                crouchInput = false;
                player.playerLocomotionManager.SwitchCrouchState();
            }
            player.playerLocomotionManager.UpdateDetectionObjectToCrouch();
        }

        private void HandleJumpInput()
        {
            if (player.currentFocusType != FocusType.Game)
            {
                return;
            }

            if (jumpInput)
            {
                jumpInput = false;

                player.playerLocomotionManager.AttemptToPerformJump();
            }
        }

        private void HandleCraftMenuInput()
        {
            if (craftMenuInput)
            {
                craftMenuInput = false;

                player.OpenCraftMenu();
            }
        }

        private void HandleInteractionInput()
        {
            if (player.currentFocusType != FocusType.Game)
            {
                return;
            }

            if (interactionInput)
            {
                interactionInput = false;

                player.characterInteractionManager.TryToInteract();
            }
        }

        private void HandleQuickUseInput()
        {
            if (player.currentFocusType != FocusType.Game)
            {
                return;
            }

            if (quickUsed1Input)
            {
                quickUsed1Input = false;
                player.characterInventoryManager.TryToUseItem(0);
            }
            else if (quickUsed2Input)
            {
                quickUsed2Input = false;
                player.characterInventoryManager.TryToUseItem(1);
            }
            else if(quickUsed3Input)
            {
                quickUsed3Input = false;
                player.characterInventoryManager.TryToUseItem(2);
            }
        }

        private void HandleFlashLightInput()
        {
            if (player.currentFocusType != FocusType.Game)
            {
                return;
            }

            if (flashlightInput)
            {
                flashlightInput = false;

                if (player.isUsingAnInteractable)
                {
                    return;
                }

                player.characterNetworkManager.flashlightOn.Value = !player.characterNetworkManager.flashlightOn.Value;
            }
        }

        private void HandleRefillFlashLightInput()
        {
            if (player.currentFocusType != FocusType.Game)
            {
                return;
            }

            if (refillFlashlightInput && !player.isUsingAnInteractable)
            {
                player.playerInventoryManager.ActivateBatterieRefill();
            }
            else
            {
                player.playerInventoryManager.StopBatterieRefill();
            }
        }

        private void HandleUseItemInput()
        {
            if (player.currentFocusType != FocusType.Game)
            {
                return;
            }

            if (useItemInput)
            {
                useItemInput = false;

                //player.playerEquipmentManager.TryToUSeEquipment();
                if(player.playerInventoryManager.currentRightHandWeapon)
                {
                    player.playerNetworkManager.SetCharacterActionHand();
                    player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.LMB_Action, player.playerInventoryManager.currentRightHandWeapon);
                }
            }
        }

        private void HandlePauseMenuInput()
        {
            if (pauseMenuInput)
            {
                pauseMenuInput = false;

                switch(player.currentFocusType)
                {
                    case FocusType.Game:
                        {
                            PlayerUIManager.instance.playerUIPauseManager.ShowPauseMenu();
                            player.currentFocusType = FocusType.UI;
                            break;
                        }
                    case FocusType.UI:
                        {
                            PlayerUIManager.instance.playerUIPauseManager.HidePauseMenu();
                            break;
                        }
                }
            }
        }
    }
}