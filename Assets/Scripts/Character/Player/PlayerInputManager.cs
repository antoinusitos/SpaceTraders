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
        private bool dodgeInput = false;
        private bool sprintInput = false;

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
            }
            else
            {
                instance.enabled = false;
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            SceneManager.activeSceneChanged += OnSceneChange;

            instance.enabled = false;
        }

        private void OnEnable()
        {
            if(playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
                playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
                playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
                playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
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
            HandleDodgeInput();
            HandleSprinting();
        }

        private void HandlePlayerMovementInput()
        {
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

            player.playerAnimatorManager.UpdateAnimatorValuesParamaters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
        }

        private void HandleCameraMovementInput()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }

        private void HandleDodgeInput()
        {
            if(dodgeInput)
            {
                dodgeInput = false;

                player.playerLocomotionManager.AttemptToPerformDodge();
            }

        }

        private void HandleSprinting()
        {
            if(sprintInput)
            {
                player.playerLocomotionManager.HandleSprinting();
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
        }
    }
}