using KBCore.Refs;
using UnityEngine;

public class CameraReticleManager : ValidatedMonoBehaviour
{
    [SerializeField, Anywhere] private Camera cam;
    [SerializeField, Anywhere] private RectTransform reticle; // your UI crosshair
    [SerializeField, Anywhere] private Canvas canvas;         // the canvas it’s on

    public Ray GetAimRay()
    {
        // For ScreenSpace-Overlay, eventCamera is null. Otherwise use canvas.worldCamera.
        Camera eventCam = (canvas && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                          ? canvas.worldCamera
                          : null;

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(eventCam, reticle.position);
        return cam.ScreenPointToRay(screenPoint);
    }
}
