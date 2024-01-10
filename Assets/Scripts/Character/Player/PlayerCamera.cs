using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance = null;
        public PlayerManager player = null;
        public Camera cameraObject = null;
        [SerializeField]
        private Transform cameraPivotTransform = null;

        [Header("Camera Settings")]
        private float cameraSmoothSpeed = 1.0f;
        [SerializeField]
        private float leftAndRightRotationSpeed = 220.0f;
        [SerializeField]
        private float upAndDownRotationSpeed = 220.0f;
        [SerializeField]
        private float minimumPivot = -30.0f;
        [SerializeField]
        private float maximumPivot = 60.0f;
        [SerializeField]
        private float cameraCollisionRadius = 0.2f;
        [SerializeField]
        private LayerMask collideWithLayers;
        [SerializeField]
        private float collisionSmoothSpeed = 0.2f;

        [Header("Camera Values")]
        private Vector3 cameraVelocity = Vector3.zero;
        private Vector3 cameraObjectPosition = Vector3.zero;
        [SerializeField]
        private float leftAndRightLookAngle = 0.0f;
        [SerializeField]
        private float upAndDownLookAngle = 0.0f;
        private float cameraZPosition = 0.0f;
        private float targetCameraZPosition = 0.0f;

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

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            cameraZPosition = cameraObject.transform.localPosition.z;
        }

        public void HandleAllCameraActions()
        {
            if (player == null)
            {
                return;
            }

            HandleFollowTarget();
            HandleRotations();
            HandleCollisions();
        }

        private void HandleFollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }

        private void HandleRotations()
        {
            Vector3 cameraRotation = Vector3.zero;
            Quaternion targetRotation = Quaternion.identity;

            leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
            upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

        private void HandleCollisions()
        {
            targetCameraZPosition = cameraZPosition;
            RaycastHit hit;
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.transform.position;
            direction.Normalize();

            if(Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
            {
                float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
            }

            if(Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
            {
                targetCameraZPosition = -cameraCollisionRadius;
            }

            cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, collisionSmoothSpeed);
            cameraObject.transform.localPosition = cameraObjectPosition;
        }
    }
}