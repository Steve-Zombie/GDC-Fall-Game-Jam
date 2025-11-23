using KBCore.Refs;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Character controller implementation of a player controller.
/// </summary>

namespace Player
{
    public class PlayerController : ValidatedMonoBehaviour
    {
        [Header("References")]
        [SerializeField, Self] private CharacterController characterController;
        [SerializeField, Anywhere] private Transform playerOrientation;
        [SerializeField, Anywhere] private Transform cameraFollow;

        [Header("Physics Settings")]
        [SerializeField] private float gravityForce = -9.8f;

        [Header("Locomotion Settings")]
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float runSpeed = 10f;

        [Header("Jump Settings")]
        [SerializeField] private float jumpHeight = 1.5f;
        [SerializeField] private bool enableAutoBHOP = false;

        [Header("Crouch Settings")]
        [SerializeField] private float crouchHeight = 1.2f;
        [SerializeField] private float standHeight = 1.8f;
        // [SerializeField] private float timeToCrouch = 0.75f;

        [Header("Actions")]
        [SerializeField, Anywhere] private InputReader inputReader;
        [SerializeField, Anywhere] private InputActionReference moveActionReference;
        [SerializeField, Anywhere] private InputActionReference runActionReference;
        [SerializeField, Anywhere] private InputActionReference jumpActionReference;
        [SerializeField, Anywhere] private InputActionReference crouchActionReference;

        [Header("Events")]
        [SerializeField] private UnityEvent<float> OnHeightChange;


        private bool _isRunning = false;
        private bool _isCrouching = false;
        private bool _shouldJump = false;
        private float _moveSpeed => _isRunning && !_isCrouching ? runSpeed : walkSpeed;
        private Vector3 verticalVelocity;
        private const float k_GroundedGravitationalForce = -1f;

        void OnEnable()
        {
            inputReader.Register(runActionReference, HandleRun);
            inputReader.Register(jumpActionReference, HandleJump);
            inputReader.Register(crouchActionReference, HandleCrouch);
        }

        void OnDisable()
        {
            inputReader.Unregister(runActionReference, HandleRun);
            inputReader.Unregister(jumpActionReference, HandleJump);
            inputReader.Unregister(crouchActionReference, HandleCrouch);
        }

        private void Update()
        {
            HandleJumpExecution();
            HandleMovement();
            HandleGravityAndVertical();
        }

        private void HandleMovement()
        {
            var dirInput = inputReader.ReadValue<Vector2>(moveActionReference);
            var moveDir = playerOrientation.right * dirInput.x + playerOrientation.forward * dirInput.y;
            if (moveDir.sqrMagnitude > 1f) moveDir.Normalize(); // optional
            var horizontalDisplacement = moveDir * _moveSpeed * Time.deltaTime;
            characterController.Move(horizontalDisplacement);
        }

        private void HandleGravityAndVertical()
        {
            verticalVelocity.y += gravityForce * Time.deltaTime;

            if (characterController.isGrounded && verticalVelocity.y < 0f)
                verticalVelocity.y = k_GroundedGravitationalForce;

            // IMPORTANT: pass displacement, not velocity
            var verticalDisplacement = verticalVelocity * Time.deltaTime;
            var flags = characterController.Move(verticalDisplacement);

            // cancel upward vel if we bonk our head
            if ((flags & CollisionFlags.Above) != 0 && verticalVelocity.y > 0f)
                verticalVelocity.y = 0f;
        }

        private void HandleRun(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _isRunning = true;
            }
            else if (context.canceled)
            {
                _isRunning = false;
            }
        }

        private void HandleJump(InputAction.CallbackContext ctx)
        {
            if (!ctx.started) return;
            if (!enableAutoBHOP && !characterController.isGrounded) return;

            _shouldJump = true;
        }

        private void HandleJumpExecution()
        {
            if (_shouldJump && characterController.isGrounded)
            {
                verticalVelocity.y = Mathf.Sqrt(2f * -gravityForce * jumpHeight);

                _shouldJump = false;
            }
        }

        private void HandleCrouch(InputAction.CallbackContext ctx)
        {
            if (ctx.started && characterController.isGrounded)
            {
                _isCrouching = true;

                AdjustControllerHeight(crouchHeight);
            }
            else if (ctx.canceled)
            {
                _isCrouching = false;

                AdjustControllerHeight(standHeight);
            }
        }

        private void AdjustControllerHeight(float height)
        {
            characterController.height = height;
            characterController.center = new Vector3(0, height / 2, 0);

            cameraFollow.localPosition = new Vector3(0, characterController.height * .8f, 0);

            OnHeightChange?.Invoke(height);
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            AdjustControllerHeight(standHeight);
        }
    }
}