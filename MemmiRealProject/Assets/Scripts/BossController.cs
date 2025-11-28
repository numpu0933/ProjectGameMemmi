using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 300;
    public int currentHealth;
    public Slider hpSlider;

    [Header("Movement")]
    public float floatSpeed = 2f;
    public float floatRange = 2f;
    private Vector3 startPos;

    [Header("Shooting")]
    public GameObject bossBulletPrefab;
    public Transform firePoint;
    public float shootInterval = 2f;
    private float shootTimer;

    public Transform player;

    [Header("Visual Feedback")]
    public SpriteRenderer spriteRenderer;
    public float flashDuration = 0.1f;
    private Coroutine flashCoroutine;

    [Header("Camera")]
    public Transform bossCameraPos;
    public float cameraTransitionSpeed = 2f;
    public float bossCameraSize = 7f;
    public float normalCameraSize = 5f;
    private bool playerInBossZone = false;

    void Start()
    {
        currentHealth = maxHealth;
        startPos = transform.position;

        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHealth;
            hpSlider.value = currentHealth;
        }

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        FloatMovement();
        HandleShooting();

        if (hpSlider != null)
            hpSlider.value = currentHealth;
    }

    void LateUpdate()
    {
        if (Camera.main == null) return;

        if (playerInBossZone && bossCameraPos != null)
        {
            Camera.main.transform.position = Vector3.Lerp(
                Camera.main.transform.position,
                new Vector3(bossCameraPos.position.x, bossCameraPos.position.y, Camera.main.transform.position.z),
                Time.deltaTime * cameraTransitionSpeed
            );

            Camera.main.orthographicSize = Mathf.Lerp(
                Camera.main.orthographicSize,
                bossCameraSize,
                Time.deltaTime * cameraTransitionSpeed
            );
        }
        else
        {
            Camera.main.transform.position = Vector3.Lerp(
                Camera.main.transform.position,
                new Vector3(player.position.x, player.position.y, Camera.main.transform.position.z),
                Time.deltaTime * cameraTransitionSpeed
            );

            Camera.main.orthographicSize = Mathf.Lerp(
                Camera.main.orthographicSize,
                normalCameraSize,
                Time.deltaTime * cameraTransitionSpeed
            );
        }
    }

    void FloatMovement()
    {
        float x = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        float y = Mathf.Cos(Time.time * floatSpeed) * floatRange * 0.5f;
        transform.position = startPos + new Vector3(x, y, 0);
    }

    void HandleShooting()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval)
        {
            ShootAtPlayer();
            shootTimer = 0f;
        }
    }

    void ShootAtPlayer()
    {
        if (player == null) return;

        Vector2 direction = (player.position - firePoint.position).normalized;

        ShootBullet(direction);
        ShootBullet(Quaternion.Euler(0, 0, 15) * direction);
        ShootBullet(Quaternion.Euler(0, 0, -15) * direction);
    }

    void ShootBullet(Vector2 dir)
    {
        GameObject bullet = Instantiate(bossBulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir * 6f;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (spriteRenderer != null)
        {
            if (flashCoroutine != null)
                StopCoroutine(flashCoroutine);

            flashCoroutine = StartCoroutine(FlashRed());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashRed()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
        flashCoroutine = null;
    }

    void Die()
    {
        Debug.Log("BOSS DEFEATED!");
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInBossZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInBossZone = false;
        }
    }
}
