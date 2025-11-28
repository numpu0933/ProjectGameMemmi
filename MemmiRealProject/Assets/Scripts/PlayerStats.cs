using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int coins = 0;
    public Slider hpSlider;
    public SpriteRenderer spriteRenderer;
    public Transform respawnPoint;
    public Image blackScreen;
    public float fadeDuration = 1f;
    public float respawnDelay = 1f;
    public float flashDuration = 0.05f;
    private Coroutine flashCoroutine;
    public float regenInterval = 2f;
    public int regenAmount = 20;

    private bool isDead = false;
    private MonoBehaviour playerControl;

    void Start()
    {
        currentHealth = maxHealth;
        playerControl = GetComponent<MonoBehaviour>();

        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHealth;
            hpSlider.value = currentHealth;
        }

        if (blackScreen != null)
            blackScreen.color = new Color(0, 0, 0, 0);

        StartCoroutine(RegenHealth());
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth < 0)
            currentHealth = 0;

        if (hpSlider != null)
            hpSlider.value = currentHealth;

        if (spriteRenderer != null)
        {
            if (flashCoroutine != null)
                StopCoroutine(flashCoroutine);

            flashCoroutine = StartCoroutine(FlashRed());
        }

        if (currentHealth <= 0)
        {
            StartCoroutine(HandleDeath());
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        if (hpSlider != null)
            hpSlider.value = currentHealth;
    }

    private IEnumerator FlashRed()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
        flashCoroutine = null;
    }

    private IEnumerator HandleDeath()
    {
        isDead = true;

        if (playerControl != null)
            playerControl.enabled = false;

        if (blackScreen != null)
        {
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                blackScreen.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, timer / fadeDuration));
                yield return null;
            }
            blackScreen.color = new Color(0, 0, 0, 1);
        }

        yield return new WaitForSeconds(respawnDelay);

        if (respawnPoint != null)
            transform.position = respawnPoint.position;

        currentHealth = maxHealth;
        if (hpSlider != null)
            hpSlider.value = currentHealth;

        if (playerControl != null)
            playerControl.enabled = true;

        if (blackScreen != null)
        {
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                blackScreen.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, timer / fadeDuration));
                yield return null;
            }
            blackScreen.color = new Color(0, 0, 0, 0);
        }

        isDead = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BossBullet"))
        {
            BossBullet bullet = other.GetComponent<BossBullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
            }
            Destroy(other.gameObject);
        }
    }

    private IEnumerator RegenHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(regenInterval);

            if (!isDead && currentHealth < maxHealth)
            {
                currentHealth += regenAmount;
                if (currentHealth > maxHealth)
                    currentHealth = maxHealth;

                if (hpSlider != null)
                    hpSlider.value = currentHealth;
            }
        }
    }
}
