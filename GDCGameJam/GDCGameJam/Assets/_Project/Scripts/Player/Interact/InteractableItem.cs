using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Any interactable object. Fires an event onInteracted when a player interacts with it.
/// </summary>
namespace Interact
{
    public class InteractableItem : Interactable
    {
        [Header("Events")]
        [SerializeField] protected UnityEvent<InteractableItem> onInteracted;

        public override string Description() => "INTERACT";

        public void RequestInteract()
        {
            onInteracted?.Invoke(this);
        }

        [ContextMenu("Test Interaction")]
        private void TestInteract()
        {
            RequestInteract();
        }
    }
}