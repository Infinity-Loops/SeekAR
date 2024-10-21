using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public static class GameObjectUtils
{
    public static void SetLayerRecursively(this GameObject inst, int layer)
    {
        inst.layer = layer;
        foreach (Transform child in inst.transform)
            child.gameObject.SetLayerRecursively(layer);
    }
}
