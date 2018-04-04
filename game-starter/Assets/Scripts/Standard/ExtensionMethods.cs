using UnityEngine;

public static class ExtensionMethods
{
    public static Transform InstantiateHere(this Transform transform, Transform prefab)
    {
        return Object.Instantiate(prefab, prefab.position + transform.position, Quaternion.identity);
    }
}