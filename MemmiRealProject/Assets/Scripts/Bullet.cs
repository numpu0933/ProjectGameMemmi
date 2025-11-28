using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 50;
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null && (other.CompareTag("Enemy") || other.CompareTag("Boss")))
        {
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }

        if (other.CompareTag("Ground"))
            Destroy(gameObject);
    }
}
