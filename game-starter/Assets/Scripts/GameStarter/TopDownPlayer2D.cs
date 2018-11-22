using UnityEngine;

namespace GameStarter
{
    public class TopDownPlayer2D : MonoBehaviour
    {
        public float accel = 50f;
        public float stoppingDrag = 25f;
        public float maxSpeed = 6f;

        Rigidbody2D rb;
        float x;
        float y;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");
        }

        void FixedUpdate()
        {
            if (Utils.IsZero(x) && Utils.IsZero(y))
            {
                rb.drag = stoppingDrag;
            }
            else
            {
                rb.drag = 0f;
                Vector2 force = new Vector2(x, y);
                force = force.normalized * accel;
                rb.AddForce(force);
            }

            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }
    }
}