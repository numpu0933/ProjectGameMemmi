using UnityEngine;
using System.Collections;

public class Enemy : EnemyBase
{
    public float moveSpeed = 2f;
    public float moveRange = 3f;
    private Vector3 startPosition;
    private int direction = 1;

    public int contactDamage = 10;
    public float attackCooldown = 1f;
    private bool canAttack = true;

    protected override void Start()
    {
        base.Start();
        startPosition = transform.position;
    }

    void Update()
    {
        MovePatrol();
    }

    private void MovePatrol()
    {
        float targetX = startPosition.x + moveRange * direction;
        Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - targetX) < 0.05f)
            direction *= -1;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!canAttack) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.TakeDamage(contactDamage);
                StartCoroutine(AttackDelay());
            }
        }
    }

    private IEnumerator AttackDelay()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        
        AudioManager.Instance.PlaySFX(AudioManager.Instance.enemyHitSound);
    }

    protected override void Die()
    {
        base.Die();
        
    }
}
