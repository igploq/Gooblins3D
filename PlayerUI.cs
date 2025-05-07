using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public Slider healthSlider;
    public Slider staminaSlider;
    public TMP_Text currencyText;

    private PlayerStats playerStats;
    private Currency playerCurrency;

    void Start()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        playerCurrency = GetComponentInParent<Currency>();

        if (playerStats != null)
        {
            healthSlider.maxValue = playerStats.maxHealth;
            staminaSlider.maxValue = playerStats.maxStamina;
        }
    }

    void Update()
    {
        if (playerStats != null)
        {
            healthSlider.value = playerStats.currentHealth;
            staminaSlider.value = playerStats.currentStamina;
        }

        if (playerCurrency != null)
        {
            currencyText.text = $"Coins: {playerCurrency.GetCurrency()}";
        }
    }
}
