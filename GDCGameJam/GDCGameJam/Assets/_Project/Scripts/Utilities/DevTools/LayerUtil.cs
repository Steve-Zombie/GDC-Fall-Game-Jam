using UnityEngine;

public static class LayerUtil
{
    public static void SetLayerRecursively(GameObject root, int layer)
    {
        if (layer < 0) return;
        foreach (var t in root.GetComponentsInChildren<Transform>(true))
            t.gameObject.layer = layer;
    }
}
