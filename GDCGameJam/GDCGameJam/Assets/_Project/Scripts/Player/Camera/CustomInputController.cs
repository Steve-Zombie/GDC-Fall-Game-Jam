using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using KBCore.Refs;
using Player;
using Unity.Cinemachine;

/// <summary>
/// Generic axis controller that reads one (or more) InputActions and feeds the values
/// to Cinemachine or whatever system your InputAxis framework uses.
/// </summary>
public class CustomInputController : InputAxisControllerBase<CustomInputController.Reader>
{
    [Header("References")]
    [SerializeField, Anywhere] private InputReader inputReader;
    [SerializeField, Anywhere] private InputActionReference lookActionReference;
    private void Update()
    {
        foreach (var ctrl in Controllers)
        {
            var reader = ctrl.Input;
            if (reader.Input == null) continue;

            reader.Poll();                // read, scale, store
        }

        if (Application.isPlaying)
            UpdateControllers();
    }

    [Serializable]
    public class Reader : IInputAxisReader
    {
        public InputActionReference Input;
        public float Gain;
        private Vector2 m_Value;

        public void Poll()
        {
            if (Input == null) return;

            var action = Input.action;
            if (action.expectedControlType == "Vector2")
                m_Value = action.ReadValue<Vector2>() * Gain;
            else
            {
                float s = action.ReadValue<float>() * Gain;
                m_Value.x = s; m_Value.y = s;
            }
        }
        
        public float GetValue(UnityEngine.Object context, IInputAxisOwner.AxisDescriptor.Hints hint)
        {
            return hint == IInputAxisOwner.AxisDescriptor.Hints.Y ? m_Value.y : m_Value.x;
        }
    }
}
