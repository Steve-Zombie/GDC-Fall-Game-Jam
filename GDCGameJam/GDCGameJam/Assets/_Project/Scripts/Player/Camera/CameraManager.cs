using System.Collections.Generic;
using Interact;
using KBCore.Refs;
using Player;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public enum CameraMode { FirstPerson }

[System.Serializable]
public struct CameraModeMapping
{
    public CameraMode mode;
    public CinemachineCamera camera;
}

public class CameraManager : ValidatedMonoBehaviour
{
    [Header("References")]
    [SerializeField, Anywhere] private InputReader inputReader;
    [SerializeField, Anywhere] private InteractionHandler _interactionHandler;
    public InteractionHandler InteractionHandler => _interactionHandler;

    [SerializeField] private Camera mainCamera;

    [Header("Camera Mode Map")]
    [SerializeField] private List<CameraModeMapping> cameraMappings;

    [Header("Settings")]
    [SerializeField] private LayerMask defaultLayerMask;

    private const int k_ActiveCamPriority = 10;
    private const int k_InactiveCamPriority = 0;


    private CameraMode _currentMode;
    private Dictionary<CameraMode, CinemachineCamera> _cameraDict;

    public CameraMode CurrentMode => _currentMode;

    void Awake()
    {
        mainCamera.cullingMask = defaultLayerMask;
        _currentMode = CameraMode.FirstPerson;

        _cameraDict = new Dictionary<CameraMode, CinemachineCamera>();
        foreach (var mapping in cameraMappings)
        {
            if (!_cameraDict.ContainsKey(mapping.mode))
            {
                _cameraDict[mapping.mode] = mapping.camera;
            }
            else
            {
                Debug.LogWarning($"Duplicate camera mapping for mode: {mapping.mode}");
            }
        }
    }

    public void ChangeCameraMode(CameraMode mode)
    {
        if (_currentMode == mode) return;

        var cam = GetCamera(mode);
        if (cam == null) return;

        DisableAllCameras();

        _currentMode = mode;
        cam.Priority = k_ActiveCamPriority;

        switch (mode)
        {
            case CameraMode.FirstPerson:
                inputReader.PushContext(InputContext.Player);
                mainCamera.cullingMask = defaultLayerMask;
                break;
        }
    }

    public CinemachineCamera GetCamera(CameraMode mode)
    {
        if (!_cameraDict.TryGetValue(mode, out var cam))
        {
            Debug.LogWarning($"No camera found for mode: {mode}");
            return null;
        }

        return cam;
    }

    private void DisableAllCameras()
    {
        foreach (var cam in _cameraDict.Values)
        {
            if (cam != null)
                cam.Priority = k_InactiveCamPriority;
        }
    }
}