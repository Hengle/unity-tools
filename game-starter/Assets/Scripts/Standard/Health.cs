using UnityEngine;

public class Health : MonoBehaviour
{

    public float currentHealth = 100;
    public float maxHealth = 100;

    public float percentHealth
    {
        get
        {
            return currentHealth / maxHealth;
        }
    }

    public AudioClip hurtClip;
    public float hurtVolume = 1f;

    public virtual void ApplyDamage(float damage)
    {
        if (currentHealth <= 0)
        {
            return;
        }

        currentHealth -= damage;

        if (hurtClip != null && damage > 0)
        {
            AudioSource.PlayClipAtPoint(hurtClip, Camera.main.transform.position, hurtVolume);
        }

        /* Kill the game obj if it loses all its health. */
        if (currentHealth <= 0)
        {
            OutOfHealth();
        }
    }

    public virtual void OutOfHealth()
    {
        Destroy(gameObject);
    }
}
