using UnityEngine;

namespace GameStarter
{
    public class TopDownPlayer2D : MonoBehaviour
    {
        public float accel = 50f;
        public float stoppingDrag = 25f;
        public float maxSpeed = 6f;

        public float knockbackAccel = 600f;

        Rigidbody2D rb;
        float x;
        float y;
        bool knockback;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");

            knockback |= Input.GetButtonDown("Jump");
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

            if(knockback) {
                rb.AddForce(Random.insideUnitCircle.normalized * knockbackAccel);
                knockback = false;
            }

            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }
    }
}