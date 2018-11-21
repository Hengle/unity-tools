using System.Collections.Generic;
using UnityEngine;

namespace GameStarter
{
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

        /// <summary>
        /// Checks if the layer is in the mask
        /// </summary>
        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }

        /// <summary>
        /// Creates a BoundsInt that contains both BoundsInts.
        /// </summary>
        public static BoundsInt Union(this BoundsInt a, BoundsInt b)
        {
            Vector3Int min = Vector3Int.Min(a.min, b.min);
            Vector3Int max = Vector3Int.Max(a.max, b.max);

            return new BoundsInt(min, max - min);
        }
    }
}