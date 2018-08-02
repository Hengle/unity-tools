using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static Transform InstantiateHere(this Transform transform, Transform prefab)
    {
        return Object.Instantiate(prefab, prefab.position + transform.position, Quaternion.identity);
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}