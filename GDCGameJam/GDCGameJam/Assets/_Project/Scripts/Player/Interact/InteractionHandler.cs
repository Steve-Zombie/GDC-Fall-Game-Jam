using KBCore.Refs;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Interaction handler for player. Checks if player is currently focusing on an object. 
/// If the player winteracts with the object, the event InteractWithInventoryItem is fired.
/// </summary>
namespace Interact
{
    public class InteractionHandler : ValidatedMonoBehaviour
    {
        [Header("References")]
        [SerializeField, Anywhere] private Camera playerCamera;
        [SerializeField, Anywhere] private CameraManager cameraManager;

        [Header("Settings")]
        [SerializeField] private float raycastDistance;
        [SerializeField] private float raycastSphereRadius = 0.05f;

        [Header("Actions")]
        [SerializeField, Anywhere] private InputReader inputReader;
        [SerializeField, Anywhere] private InputActionReference interactionActionReference;

        public event UnityAction<Interactable> OnFocus = delegate { }; // Passes focused item
        public event UnityAction<Interactable> OnFocusLost = delegate { }; // Passes previously focused item
        public bool IsFocused => FocusedItem != null;
        public Interactable FocusedItem { get; private set; }

        void OnEnable()
        {
            inputReader.Register(interactionActionReference, HandleOnInteract);
        }

        void OnDisable()
        {
            inputReader.Unregister(interactionActionReference, HandleOnInteract);
        }

        private void HandleOnInteract(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnInteract();
            }
        }

        private void Update()
        {
            HandleRaycast();
        }

        /// <summary>
        /// Performs a spherecast from the player's camera to detect and track focus on interactable objects.
        /// Handles enabling/disabling outlines and invoking focus change events.
        /// Automatically exits and clears focus if not in first-person camera mode.
        /// </summary>
        public void HandleRaycast()
        {
            if (cameraManager.CurrentMode != CameraMode.FirstPerson)
            {
                if (FocusedItem != null)
                {
                    OnFocusLost.Invoke(FocusedItem);
                    FocusedItem = null;
                }

                return;
            }

            var rayOrigin = playerCamera.transform.position;
            var rayDirection = playerCamera.transform.forward;

            var previousItem = FocusedItem;
            FocusedItem = null;

            if (Physics.SphereCast(rayOrigin, raycastSphereRadius, rayDirection, out var info, raycastDistance) &&
            info.collider.TryGetComponent<Interactable>(out var item) && item.IsInteractable)
            {
                FocusedItem = item;
            }

            if (previousItem == null && FocusedItem != null)
            {
                OnFocus.Invoke(FocusedItem);
            }
            else if (previousItem != null && FocusedItem == null)
            {
                OnFocusLost.Invoke(previousItem);
            }
        }

        private void OnInteract()
        {
            if (FocusedItem is InteractableItem interactableItem)
            {
                interactableItem.RequestInteract();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * raycastDistance);
        }
    }
}