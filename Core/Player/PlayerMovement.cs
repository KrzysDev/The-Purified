using Unity.Cinemachine;
using UnityEngine;

namespace ThePurified.PlayerSystem 
{
    /// <summary>
    /// Klasa implementujaca poruszanie sie pierwszoosobowe gracza.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        private CharacterController controller;

        private float xInput, zInput;

        [Header("PlayerCamera reference")]
        [SerializeField] Transform playerCamera;
        public static CinemachineCamera cinemachineCamera; //uzywane z innych skryptow aby wylaczac kamere gdy gracz nie powinien nia ruszac

        [Space(3)]
        [Header("Walk Speed")]
        [Tooltip("How fast player walks")]
        [SerializeField] float walkSpeed;
        [Space(1)]
        [Header("Run Speed")]
        [Tooltip("How fast player runs")]
        [SerializeField] float runSpeed;

        public static bool movementEnabled = true;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            LockCursor();

            cinemachineCamera = playerCamera.GetComponent<CinemachineCamera>();
        }

        void Update()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            if (movementEnabled)
            {
                xInput = Input.GetAxis("Horizontal");
                zInput = Input.GetAxis("Vertical");

                Vector3 right = playerCamera.transform.right;
                Vector3 forward = playerCamera.transform.forward;

                forward.y = right.y = 0;
                right.Normalize();
                forward.Normalize();

                Vector3 direction = right * xInput + forward * zInput;

                float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

                controller.Move(speed * direction * Time.deltaTime);
            }
        }

        private void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}

