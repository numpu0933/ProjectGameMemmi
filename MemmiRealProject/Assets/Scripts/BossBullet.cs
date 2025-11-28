using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public int damage = 20;
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null && other.CompareTag("Player"))
        {
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }

        if (other.CompareTag("Ground"))
            Destroy(gameObject);
    }
}
