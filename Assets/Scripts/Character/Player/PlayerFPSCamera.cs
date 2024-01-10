using UnityEngine;

namespace AG
{
    public class PlayerFPSCamera : MonoBehaviour
    {
        public static PlayerFPSCamera instance = null;
        public PlayerManager player = null;
        [SerializeField]
        private Transform cameraPivotTransform = null;

        [Header("Camera Settings")]
        [SerializeField]
        private float leftAndRightRotationSpeed = 220.0f;
        [SerializeField]
        private float upAndDownRotationSpeed = 120.0f;
        [SerializeField]
        private float minimumPivot = -80.0f;
        [SerializeField]
        private float maximumPivot = 80.0f;

        [Header("Camera Values")]
        [SerializeField]
        private float upAndDownLookAngle = 0.0f;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            transform.Rotate(0, PlayerInputManager.instance.cameraHorizontalInput * Time.deltaTime * leftAndRightRotationSpeed, 0);

            upAndDownLookAngle -= PlayerInputManager.instance.cameraVerticalInput * Time.deltaTime * upAndDownRotationSpeed;
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);
            cameraPivotTransform.localRotation = Quaternion.Euler(upAndDownLookAngle, 0, 0);
        }
    }
}