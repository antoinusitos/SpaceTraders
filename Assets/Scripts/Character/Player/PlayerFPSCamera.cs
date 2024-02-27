using System;
using UnityEngine;

namespace AG
{
    public class PlayerFPSCamera : MonoBehaviour
    {
        private PlayerManager player = null;

        public Camera playerCamera = null;

        [SerializeField]
        private Transform cameraPivotTransform = null;

        [Header("Camera Settings")]
        [SerializeField]
        private float leftAndRightRotationSpeed = 50;
        [SerializeField]
        private float upAndDownRotationSpeed = 50;
        [SerializeField]
        private float minimumPivot = -80.0f;
        [SerializeField]
        private float maximumPivot = 80.0f;

        [SerializeField]
        private float standingYPosition = 1.7f;
        [SerializeField]
        private float crouchingYPosition = 1.2f;

        [Header("Camera Values")]
        [SerializeField]
        private float upAndDownLookAngle = 0.0f;

        private float invertY = 1.0f;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            WorldSettingsManager.instance.OnFOVChanged += OnFOVChanged;
            WorldSettingsManager.instance.OnVerticalCameraSpeedChanged += OnHorizontalCameraSpeedChanged;
            WorldSettingsManager.instance.OnVerticalCameraSpeedChanged += OnVerticalCameraSpeedChanged;
            WorldSettingsManager.instance.OnInvertYChanged += OnInvertYChanged;

            OnFOVChanged(WorldSettingsManager.instance.GetFOV());
            OnHorizontalCameraSpeedChanged(WorldSettingsManager.instance.GetHorizontalCameraSpeed());
            OnVerticalCameraSpeedChanged(WorldSettingsManager.instance.GetVerticalCameraSpeed());
            OnInvertYChanged(WorldSettingsManager.instance.GetInvertY());
        }

        private void OnDestroy()
        {
            WorldSettingsManager.instance.OnFOVChanged -= OnFOVChanged;
            WorldSettingsManager.instance.OnVerticalCameraSpeedChanged -= OnHorizontalCameraSpeedChanged;
            WorldSettingsManager.instance.OnVerticalCameraSpeedChanged -= OnVerticalCameraSpeedChanged;
            WorldSettingsManager.instance.OnInvertYChanged -= OnInvertYChanged;
        }

        private void OnInvertYChanged(bool newValue)
        {
            if (newValue)
            {
                invertY = -1;
            }
            else
            {
                invertY = 1;
            }
        }

        private void OnVerticalCameraSpeedChanged(float newValue)
        {
            upAndDownRotationSpeed = newValue;
        }

        private void OnHorizontalCameraSpeedChanged(float newValue)
        {
            leftAndRightRotationSpeed = newValue;
        }

        private void OnFOVChanged(float newValue)
        {
            playerCamera.fieldOfView = newValue;
        }

        private void Update()
        {
            if(!player.IsOwner)
            {
                return;
            }

            if(!player.canRotate)
            {
                return;
            }

            if (player.isUsingAnInteractable)
            {
                return;
            }

            if (player.playerNetworkManager.isCrouching.Value)
            {
                cameraPivotTransform.localPosition = Vector3.up * crouchingYPosition;
            }
            else
            {
                cameraPivotTransform.localPosition = Vector3.up * standingYPosition;
            }

            if (player.isInMenu)
            {
                return;
            }

            transform.Rotate(0, PlayerInputManager.instance.cameraHorizontalInput * Time.deltaTime * leftAndRightRotationSpeed, 0);

            upAndDownLookAngle -= PlayerInputManager.instance.cameraVerticalInput * Time.deltaTime * upAndDownRotationSpeed * invertY;
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);
            cameraPivotTransform.localRotation = Quaternion.Euler(upAndDownLookAngle, 0, 0);

            player.playerNetworkManager.cameraUpDownAngle.Value = upAndDownLookAngle;
        }
    }
}