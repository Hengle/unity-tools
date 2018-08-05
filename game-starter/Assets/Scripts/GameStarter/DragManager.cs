using UnityEngine;

namespace GameStarter
{
    public class DragManager : Manager<DragManager>
    {
        public int dragThreshold = 5;

        public bool isDragging = false;

        public Vector2 dragDelta;

        private Vector3 lastMousePos;

        private float dragDist = 0;

        public void StartLevel()
        {
            if (Input.GetMouseButton(0))
            {
                lastMousePos = Input.mousePosition;
            }
        }

        void Update()
        {
            dragDelta = Vector2.zero;

            if (Input.GetMouseButtonDown(0))
            {
                dragDist = 0;
                lastMousePos = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                dragDist += (Input.mousePosition - lastMousePos).magnitude;

                if (!isDragging && dragDist > DpiBasedDragThreshold())
                {
                    isDragging = true;
                }

                dragDelta = (Input.mousePosition - lastMousePos) / Screen.height;

                lastMousePos = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
        }

        /// <summary>
        /// Gets the drag threshold in pixels based off the Screen dpi and default
        /// drag threshold for a 160 DPI screen.
        /// </summary>
        private int DpiBasedDragThreshold()
        {
            return Mathf.Max(dragThreshold, (int)(dragThreshold * Screen.dpi / 160f));
        }
    }
}