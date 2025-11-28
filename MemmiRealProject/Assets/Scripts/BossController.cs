using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossController : EnemyBase
{
    public Slider hpSlider;
    public float floatSpeed = 2f;
    public float floatRange = 2f;
    private Vector3 startPos;

    public GameObject bossBulletPrefab;
    public Transform firePoint;
    public float shootInterval = 2f;
    private float shootTimer;
    public Transform player;

    public Transform bossCameraPos;
    public float cameraTransitionSpeed = 2f;
    public float bossCameraSize = 7f;
    public float normalCameraSize = 5f;
    private bool playerInBossZone = false;

    public GameObject winPanel;

    protected override void Start()
    {
        base.Start();
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

        if (AudioManager.Instance != null && AudioManager.Instance.shootSound != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.shootSound);
    }

    void ShootBullet(Vector2 dir)
    {
        GameObject bullet = Instantiate(bossBulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir * 6f;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (AudioManager.Instance != null && AudioManager.Instance.bossHitSound != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.bossHitSound);

        if (currentHealth <= 0)
            StartCoroutine(SlowMotionAndStop());
    }

    protected override void Die()
    {
        if (AudioManager.Instance != null && AudioManager.Instance.bossDeathSound != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.bossDeathSound);

        if (winPanel != null)
            winPanel.SetActive(true);

        if (AudioManager.Instance != null && AudioManager.Instance.winMusic != null)
            AudioManager.Instance.PlayMusic(AudioManager.Instance.winMusic);

        base.Die();
    }

    private IEnumerator SlowMotionAndStop()
    {
        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0.02f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BossZone"))
            playerInBossZone = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("BossZone"))
            playerInBossZone = false;
    }
}
