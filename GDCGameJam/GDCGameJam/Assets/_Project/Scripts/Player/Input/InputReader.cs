using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Flags]
public enum InputContext
{
    None = 0,      // All maps inactive
    Player = 1,
}

public class InputReader : ScriptableObject
{
    [SerializeField] private InputActionAsset actions;
    [SerializeField] private InputContext defaultContext;

    private readonly Dictionary<InputContext, InputActionMap> _maps = new();
    private readonly Stack<InputContext> _contextStack = new();

    void OnEnable()
    {
        _maps.Clear();
        foreach (InputContext ctx in Enum.GetValues(typeof(InputContext)))
        {
            var map = actions.FindActionMap(ctx.ToString(), throwIfNotFound: false);
            if (map != null) _maps.Add(ctx, map);
        }

        _contextStack.Clear();
        PushContext(defaultContext);
    }

    void OnDisable()
    {
        while (_contextStack.Count > 0)
            DisableContext(_contextStack.Pop());
    }

    private void EnableContext(InputContext ctx)
    {
        if (ctx == InputContext.None) return;
        if (_maps.TryGetValue(ctx, out var map)) map.Enable();
        else Printer.PrintWarning("Input", $"Context '{ctx}' isn’t cached.");
    }

    private void DisableContext(InputContext ctx)
    {
        if (ctx == InputContext.None) return;
        if (_maps.TryGetValue(ctx, out var map)) map.Disable();
    }

    public void PushContext(InputContext ctx)
    {
        if (_contextStack.Count > 0)
            DisableContext(_contextStack.Peek());

        _contextStack.Push(ctx);
        EnableContext(ctx);
    }

    public void PopContext()
    {
        if (_contextStack.Count == 0) return;

        DisableContext(_contextStack.Pop());

        if (_contextStack.Count > 0)
            EnableContext(_contextStack.Peek());   // does nothing if top is None
    }

    // Optional convenience
    public void PushNone() => PushContext(InputContext.None);

    public void Register(InputActionReference actionRef,
                     Action<InputAction.CallbackContext> cb)
    {
        if (actionRef?.action == null)
        {
            Printer.PrintWarning("Input", "Register called with null InputActionReference.");
            return;
        }

        if (!Enum.TryParse<InputContext>(actionRef.action.actionMap.name, out var ctx))
        {
            Printer.PrintWarning("Input", $"No InputContext enum matches map '{actionRef.action.actionMap.name}'.");
            return;
        }

        if (!_maps.TryGetValue(ctx, out var map))
        {
            Printer.PrintWarning("Input", $"Context '{ctx}' wasn’t cached in _maps.");
            return;
        }

        map[actionRef.action.name].started += cb;
        map[actionRef.action.name].performed += cb;
        map[actionRef.action.name].canceled += cb;
    }

    public void Unregister(InputActionReference actionRef,
                       Action<InputAction.CallbackContext> cb)
    {
        if (actionRef?.action == null)
        {
            Printer.PrintWarning("Input", "Unregister called with null InputActionReference.");
            return;
        }

        if (!Enum.TryParse<InputContext>(actionRef.action.actionMap.name, out var ctx))
        {
            Printer.PrintWarning("Input",
                $"No InputContext enum matches map '{actionRef.action.actionMap.name}'.");
            return;
        }

        if (!_maps.TryGetValue(ctx, out var map))
        {
            Printer.PrintWarning("Input", $"Context '{ctx}' wasn’t cached in _maps.");
            return;
        }

        map[actionRef.action.name].started -= cb;
        map[actionRef.action.name].performed -= cb;
        map[actionRef.action.name].canceled -= cb;
    }

    public T ReadValue<T>(InputActionReference actionRef) where T : struct
    {
        // Basic null-guard
        if (actionRef?.action == null)
        {
            Printer.PrintWarning("Input",
                "ReadValue called with null InputActionReference.");
            return default;
        }

        // Which context/map does this action live in?
        if (!Enum.TryParse<InputContext>(actionRef.action.actionMap.name,
                                         out var ctx) ||
            !_maps.TryGetValue(ctx, out var map))
        {
            Printer.PrintWarning("Input",
                $"Action map '{actionRef.action.actionMap.name}' isn’t cached.");
            return default;
        }

        var action = map[actionRef.action.name];

        return action.ReadValue<T>();
    }
}
