using UnityEngine;

namespace GameStarter
{
    public static class Utils
    {
        public const float EPSILON = 0.00001f;

        public static T GetComponentAtMouse3D<T>()
        {
            T target = default(T);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
            {
                target = hit.transform.GetComponent<T>();
            }

            return target;
        }

        public static T GetComponentAtMouse2D<T>()
        {
            T target = default(T);

            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D col = Physics2D.OverlapPoint(worldPoint);

            if (col != null)
            {
                target = col.GetComponent<T>();
            }

            return target;
        }

        public static bool IsZero(float f)
        {
            return System.Math.Abs(f) < EPSILON;
        }

        public static float Sign(float f)
        {
            return IsZero(f) ? 0 : Mathf.Sign(f);
        }
    }
}