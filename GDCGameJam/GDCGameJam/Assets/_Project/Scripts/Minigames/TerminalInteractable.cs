using KBCore.Refs;
using UnityEngine;

public class TerminalInteractable : MonoBehaviour
{
    [Header("References")]
    [SerializeField, Self] private MeshRenderer meshRenderer;

    [Header("Settings")]
    [SerializeField] private Color colorA = Color.green;
    [SerializeField] private Color colorB = Color.red;

    private bool _toggle;

    void OnValidate()
    {
        meshRenderer.sharedMaterial.color = colorA;
    }

    public void OnInteract()
    {
        _toggle = !_toggle;
        meshRenderer.sharedMaterial.color = _toggle ? colorA : colorB;
    }
}
