using UnityEngine;

namespace GameStarter
{
    public class ThirdPersonPlayer : MonoBehaviour
    {
        public float speed = 5f;

        public float facingSpeed = 540f;

        public float jumpSpeed = 6f;

        public float jumpThreshold = 0.1f;

        public bool autoAttachToCamera = true;

        private const float OFFSET = 0.01f;

        private Rigidbody rb;

        private SphereCollider col;

        private Transform cam;

        private Transform modelHolder;

        private float horAxis = 0f;

        private float vertAxis = 0f;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<SphereCollider>();
            cam = Camera.main.transform;
            modelHolder = transform.Find("ModelHolder");

            if (autoAttachToCamera)
            {
                cam.GetComponent<ThirdPersonCamera>().target = transform;
            }
        }

        void Update()
        {
            horAxis = Input.GetAxisRaw("Horizontal");
            vertAxis = Input.GetAxisRaw("Vertical");

            if (Input.GetButtonDown("Jump"))
            {
                Vector3 origin = transform.position + col.center + (Vector3.up * OFFSET);
                RaycastHit hit;

                if (Physics.SphereCast(origin, col.radius, Vector3.down, out hit)
                    && (hit.distance - OFFSET) < jumpThreshold)
                {
                    Vector3 vel = rb.velocity;
                    vel.y = jumpSpeed;
                    rb.velocity = vel;
                }
            }
        }

        void FixedUpdate()
        {
            float y = rb.velocity.y;

            if (Cursor.lockState == CursorLockMode.Locked)
            {
                rb.velocity = GetMovementDir() * speed;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }

            Vector3 vel = rb.velocity;
            vel.y = y;
            rb.velocity = vel;
        }

        void LateUpdate()
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Vector3 dir = GetMovementDir();

                if (dir.magnitude > 0)
                {
                    float curFacing = modelHolder.eulerAngles.y;
                    float facing = Mathf.Atan2(-dir.z, dir.x) * Mathf.Rad2Deg;
                    modelHolder.rotation = Quaternion.Euler(0, Mathf.MoveTowardsAngle(curFacing, facing, facingSpeed * Time.deltaTime), 0);
                }
            }
        }

        private Vector3 GetMovementDir()
        {
            return ((Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized * vertAxis) + (cam.right * horAxis)).normalized;
        }
    }
}