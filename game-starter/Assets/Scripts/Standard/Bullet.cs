using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 velocity;
    public float damage;

    private Rigidbody rb;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = velocity;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Enemy")
        {
            if (other.tag == "Player")
            {
                Health h = other.GetComponent<Health>();
                h.ApplyDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
