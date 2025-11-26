using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;        // ความเร็วศัตรู
    public float moveRange = 3f;        // ระยะทางเดินซ้ายขวา
    private Vector3 startPosition;
    private int direction = 1;          // 1 = ขวา, -1 = ซ้าย

    void Start()
    {
        startPosition = transform.position; // จุดเริ่มต้น
    }

    void Update()
    {
        // คำนวณตำแหน่งเป้าหมาย
        float targetX = startPosition.x + moveRange * direction;
        Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

        // เดินแบบ Smooth
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // ถ้าใกล้เป้าหมาย ให้กลับทิศ
        if (Mathf.Abs(transform.position.x - targetX) < 0.05f)
            direction *= -1;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Player ตายทันที
            PlayerStats stats = collision.gameObject.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(stats.currentHealth); // ทำให้ HP = 0
            }
        }
    }
}
