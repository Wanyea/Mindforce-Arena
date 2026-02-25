using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles the health for the player as well as updating the UI component
/// </summary>
public class HealthBar : MonoBehaviour
{
    public enum PlayerLivesOptions
    {
        playerOneLives,
        playerTwoLives
    }
    public PlayerLivesOptions selectedPlayerLives;
    public PlayerLives playerLives;
    private int health;
    public Image barImage;
    public TMP_Text healthText;

    public int MaxHealth { get; private set; }

    private void Start()
    {
        MaxHealth = 10;
        health = MaxHealth;
        UpdateHealthBar();
    }

    private void Update()
    {
        switch(selectedPlayerLives)
        {
            case PlayerLivesOptions.playerOneLives:
                health = playerLives.playerOneLives.Value;
                UpdateHealthBar();
                break;
            case PlayerLivesOptions.playerTwoLives:
                health = playerLives.playerTwoLives.Value;
                UpdateHealthBar();
                break;
        }
    }

    /// <summary>
    /// sets the health
    /// </summary>
    /// <param name="newHealth">
    /// refers to the value of the new health being set
    /// </param>
    public void SetHealth(int newHealth)
    {
        health = Mathf.Clamp(newHealth, 0, MaxHealth);
        UpdateHealthBar();
    }

    /// <summary>
    /// subtracts health from the player equal to the damage value
    /// </summary>
    /// <param name="damage">
    /// refers to the amount of health lost
    /// </param>
    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, MaxHealth);
        UpdateHealthBar();
    }

    /// <summary>
    /// adds health to the player for a heal amount
    /// </summary>
    /// <param name="amount">
    /// refers to the amount of health gained
    /// </param>
    public void Heal(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, MaxHealth);
        UpdateHealthBar();
    }

    /// <summary>
    /// visually update the health bar and text to match the current amount
    /// </summary>
    private void UpdateHealthBar()
    {
        float healthNormalized = (float)health / MaxHealth;
        barImage.fillAmount = healthNormalized;
        healthText.text = health.ToString();
    }
}
