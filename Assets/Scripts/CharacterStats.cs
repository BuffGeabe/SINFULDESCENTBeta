using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public string characterName;
    public int level = 1;
    public int maxHealth;
    public int currentHealth;
    public int attackPower;
    public int defensePower;
    public int currentXP = 0;
    public int xpToNextLevel = 100;

    public int currency = 0;

    void Start()
    {
        currentHealth = maxHealth;
        ClearCurrency(); // Clear currency for testing
        currency = 0; // Initialize currency to 0
        LoadCurrency();
        UpdateCurrencyUI(); // Initialize currency UI
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"{characterName} takes {damage} damage.");
        currentHealth -= Mathf.Max(0, damage - defensePower);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"{characterName}'s current health: {currentHealth}/{maxHealth}");
    }

    public void GainXP(int xp)
    {
        currentXP += xp;
        while (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentXP -= xpToNextLevel;
        level++;
        xpToNextLevel = Mathf.FloorToInt(xpToNextLevel * 1.5f); // Increase XP needed for next level

        // Improve stats on level up
        maxHealth += 10;
        attackPower += 2;
        defensePower += 1;
        currentHealth = maxHealth; // Restore health on level up

        Debug.Log(characterName + " leveled up to level " + level + "!");
    }

    public void GainCurrency(int amount)
    {
        currency += amount;
        Debug.Log(characterName + " gained " + amount + " currency. Total: " + currency);
        SaveCurrency();
        UpdateCurrencyUI(); // Update the currency UI
    }

    public void SaveCurrency()
    {
        PlayerPrefs.SetInt(characterName + "_currency", currency);
        PlayerPrefs.Save();
    }

    public void LoadCurrency()
    {
        if (PlayerPrefs.HasKey(characterName + "_currency"))
        {
            currency = PlayerPrefs.GetInt(characterName + "_currency");
        }
    }

    private void UpdateCurrencyUI()
    {
        if (FindObjectOfType<PlayerMovement>() != null)
        {
            FindObjectOfType<PlayerMovement>().UpdateCurrencyUI();
        }
    }

    public void ClearCurrency()
    {
        PlayerPrefs.DeleteKey(characterName + "_currency");
        currency = 0;
        UpdateCurrencyUI();
    }
}
