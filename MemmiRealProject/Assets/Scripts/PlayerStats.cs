using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Stats")]
    public int maxHealth = 100;
    public int currentHealth;
    public int coins = 0;

    [Header("Respawn Settings")]
    public Transform respawnPoint; // จุดเริ่มต้น respawn
    public Image blackScreen;      // UI Panel สีดำครอบหน้าจอ
    public float fadeDuration = 1f; // เวลา fade in/out
    public float respawnDelay = 1f; // เวลาหลังหน้าจอดำก่อน respawn

    private bool isDead = false;
    private MonoBehaviour playerControl; // Script ควบคุม Player เช่น PlayerMove

    void Start()
    {
        currentHealth = maxHealth;

        // หา Script ควบคุม Player (สมมติว่า PlayerMove)
        playerControl = GetComponent<MonoBehaviour>();

        // เริ่มหน้าจอโปร่งใส
        if (blackScreen != null)
            blackScreen.color = new Color(0, 0, 0, 0);
    }

    /// เรียกเพื่อให้ Player โดนความเสียหาย
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            StartCoroutine(HandleDeath());
        }
    }

    /// ฟื้น HP
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    /// ระบบตาย + Respawn
    IEnumerator HandleDeath()
    {
        isDead = true;

        // ปิดการควบคุม Player
        if (playerControl != null)
            playerControl.enabled = false;

        // Fade หน้าจอเป็นดำ
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

        // รอเวลาสั้น ๆ ก่อน Respawn
        yield return new WaitForSeconds(respawnDelay);

        // ย้าย Player ไปจุด Respawn
        if (respawnPoint != null)
            transform.position = respawnPoint.position;

        // ฟื้นฟู HP เต็ม
        currentHealth = maxHealth;

        // เปิดการควบคุม Player
        if (playerControl != null)
            playerControl.enabled = true;

        // Fade กลับหน้าจอโปร่งใส
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
}
