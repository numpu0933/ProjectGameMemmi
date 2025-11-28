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
        if (other.CompareTag("Player"))
        {
            PlayerStats player = other.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
