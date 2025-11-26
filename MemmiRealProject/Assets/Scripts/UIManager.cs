using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Player Stats UI")]
    public Slider healthBar;
    public TextMeshProUGUI coinText; // ถ้าใช้ Text ให้เปลี่ยนเป็น Text
    public TextMeshProUGUI gameOverText;

    public PlayerStats playerStats;

    void Start()
    {
        if (playerStats != null && healthBar != null)
        {
            healthBar.maxValue = playerStats.maxHealth;
            healthBar.value = playerStats.currentHealth;
        }

        if (gameOverText != null)
            gameOverText.alpha = 0; // เริ่มซ่อน
    }

    void Update()
    {
        if (playerStats == null) return;

        // อัพเดท Health Bar
        if (healthBar != null)
            healthBar.value = playerStats.currentHealth;

        // อัพเดท Coin
        if (coinText != null)
            coinText.text = "Coins: " + playerStats.coins;

        // Game Over
        if (playerStats.currentHealth <= 0 && gameOverText != null)
        {
            gameOverText.alpha = 1;
        }
    }
}
