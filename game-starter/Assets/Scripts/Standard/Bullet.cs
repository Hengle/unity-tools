using UnityEngine;

public class Bullet : MonoBehaviour
{
    public string hurtableTag;
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
        if (other.tag == hurtableTag)
        {
            Health h = other.GetComponent<Health>();
            h.ApplyDamage(damage);
        }

        Destroy(gameObject);
    }
}
