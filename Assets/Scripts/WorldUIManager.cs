using UnityEngine;
using UnityEngine.UI;

public class WorldUIManager : MonoBehaviour
{
    public Text currencyText; // Reference to the UI Text element

    private CharacterStats playerStats;

    void Start()
    {
        // Assuming you have a way to get the player's stats, e.g., finding the player object
        playerStats = FindObjectOfType<CharacterStats>();

        // Initialize the currency display
        UpdateCurrencyDisplay();

        // Optional: if you have events or a system to notify changes in currency, subscribe to them
        // Example: playerStats.OnCurrencyChanged += UpdateCurrencyDisplay;
    }

    public void UpdateCurrencyDisplay()
    {
        if (playerStats != null)
        {
            currencyText.text = $"Currency: {playerStats.currency}";
        }
    }
}
