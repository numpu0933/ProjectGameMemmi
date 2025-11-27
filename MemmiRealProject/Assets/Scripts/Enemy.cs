using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveRange = 3f;
    private Vector3 startPosition;
    private int direction = 1;

    public int maxHealth = 500;
    private int currentHealth;

    private SpriteRenderer spriteRenderer;
    public float flashDuration = 0.1f;

    void Start()
    {
        startPosition = transform.position;
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float targetX = startPosition.x + moveRange * direction;
        Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - targetX) < 0.05f)
            direction *= -1;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashRed());
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = Color.white;
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
