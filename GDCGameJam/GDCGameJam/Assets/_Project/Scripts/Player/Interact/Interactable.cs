using KBCore.Refs;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Parent class for any interactable object. Provides outline as well as 
/// interaction descriptions when player hovers over the object.
/// 
/// Custom inspector for easy setup. Just put this script on the object and click generate outline.
/// </summary>
namespace Interact
{
    public abstract class Interactable : ValidatedMonoBehaviour
    {
        [Header("Interactable References")]
        [SerializeField, Child] protected Collider col;

        protected bool _isInteractable = true;
        public bool IsInteractable => _isInteractable;
        public abstract string Description();
        private UnityEvent onHoverEnter;
        private UnityEvent onHoverExit;

        public void EnableNotif(bool active)
        {
            if (active)
            {
                onHoverEnter.Invoke();
            }
            else
            {
                onHoverExit.Invoke();
            }
        }

        public void DisableInteraction()
        {
            _isInteractable = false;
        }

        public void EnableInteraction()
        {
            _isInteractable = true;
        }
    }

}