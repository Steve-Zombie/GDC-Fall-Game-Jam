using System.Collections.Generic;
using KBCore.Refs;
using Unity.Cinemachine;
using UnityEngine;
public class CameraLook : ValidatedMonoBehaviour, IInputAxisOwner
{
    [Header("References")]
    [SerializeField, Self] private CustomInputController inputController;
    [SerializeField, Anywhere] private Transform camOrientation;
    [SerializeField, Anywhere] private Transform playerOrientation;

    [Header("Settings")]
    [SerializeField, Range(0, 1)] private float sensitivity = 0.5f;

    private InputAxis _pan = DefaultPan;
    private InputAxis _tilt = DefaultTilt;

    private const string k_PanName = "Look X (Pan)";
    private const string k_TiltName = "Look Y (Tilt)";

    static InputAxis DefaultPan => new()
    { Value = 0, Range = new Vector2(-180, 180), Wrap = true, Center = 0, Restrictions = InputAxis.RestrictionFlags.NoRecentering };
    static InputAxis DefaultTilt => new()
    { Value = 0, Range = new Vector2(-89, 89), Wrap = false, Center = 0, Restrictions = InputAxis.RestrictionFlags.NoRecentering };
    public void GetInputAxes(List<IInputAxisOwner.AxisDescriptor> axes)
    {
        axes.Add(new() { DrivenAxis = () => ref _pan, Name = k_PanName, Hint = IInputAxisOwner.AxisDescriptor.Hints.X });
        axes.Add(new() { DrivenAxis = () => ref _tilt, Name = k_TiltName, Hint = IInputAxisOwner.AxisDescriptor.Hints.Y });
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        _pan.Validate();
        _tilt.Range.x = Mathf.Clamp(_tilt.Range.x, -90, 90);
        _tilt.Range.y = Mathf.Clamp(_tilt.Range.y, -90, 90);
        _tilt.Validate();

        foreach (var c in inputController.Controllers)
        {
            if (c.Name == k_PanName)
            {
                c.Input.Gain = sensitivity;
            }
            else if (c.Name == k_TiltName)
            {
                c.Input.Gain = -sensitivity;
            }
        }
    }

    private void Reset()
    {
        _pan = DefaultPan;
        _tilt = DefaultTilt;
    }

    private void OnEnable()
    {
        var euler = transform.rotation.eulerAngles;
        _pan.Value = euler.y;
        _tilt.Value = euler.x;
    }

    private void Update()
    {
        // Apply motion
        playerOrientation.rotation = Quaternion.Euler(0, _pan.Value, 0);
        camOrientation.rotation = Quaternion.Euler(_tilt.Value, _pan.Value, 0);
    }
}
