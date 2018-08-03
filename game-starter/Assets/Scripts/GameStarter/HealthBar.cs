using UnityEngine;

namespace GameStarter
{
    public class HealthBar : MonoBehaviour
    {
        public Transform prefab;
        public Vector3 spacing;
        public int num = 6;

        void Start()
        {
            if (transform.childCount == 0)
            {
                SetNum(num);
            }
        }

        public void SetNum(int n)
        {
            num = n;

            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            for (int i = 0; i < num; i++)
            {
                Transform t = Instantiate(prefab, transform).transform;
                t.localPosition = spacing * i;
            }
        }

    }
}