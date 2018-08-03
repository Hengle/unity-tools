using UnityEngine;
using UnityMovementAI;

namespace GameStarter
{
    public class TopDownPlayer : MonoBehaviour
    {
        public float speed = 21f;
        public float turnSmooth = 0.3f;
        public float maxTurnSpeed = 540f;

        public Transform model;

        private float horAxis = 0f;
        private float vertAxis = 0f;
        private float turnVelY = 0.0F;

        private MovementAIRigidbody rb;

        void Start()
        {
            rb = GetComponent<MovementAIRigidbody>();
        }

        void Update()
        {
            horAxis = Input.GetAxisRaw("Horizontal");
            vertAxis = Input.GetAxisRaw("Vertical");
            RotateChar();
        }

        private void RotateChar()
        {
            if (horAxis != 0 || vertAxis != 0)
            {
                float targetYAngle = Mathf.Atan2(-vertAxis, horAxis) * Mathf.Rad2Deg;
                Vector3 eulerAngles = model.localEulerAngles;
                eulerAngles.y = Mathf.SmoothDampAngle(eulerAngles.y, targetYAngle, ref turnVelY, turnSmooth, maxTurnSpeed);
                model.localEulerAngles = eulerAngles;
            }
        }

        void FixedUpdate()
        {
            RotateChar();
            MoveChar();
        }

        private void MoveChar()
        {
            Vector3 vel = (transform.right * horAxis) + (transform.forward * vertAxis);
            vel = vel.normalized * speed;
            rb.velocity = vel;
        }
    }
}