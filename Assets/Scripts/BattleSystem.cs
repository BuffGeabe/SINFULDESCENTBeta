using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    public Camera worldCamera;
    public Camera battleCamera;
    public GameObject battleUI;

    public Button attackButton;
    public Button itemButton;
    public Button defendButton;
    public Button escapeButton;

    public Text playerHealthText;
    public Text enemyHealthText;

    public CharacterStats playerStats;
    public CharacterStats enemyStats;

    private bool isPlayerTurn = true;

    void Start()
    {
        battleUI.SetActive(false);

        attackButton.onClick.AddListener(OnAttack);
        itemButton.onClick.AddListener(OnItem);
        defendButton.onClick.AddListener(OnDefend);
        escapeButton.onClick.AddListener(OnEscape);
    }

    public void TriggerBattle(CharacterStats player, CharacterStats enemy)
    {
        Debug.Log("Battle begins!");

        playerStats = player;
        enemyStats = enemy;

        worldCamera.gameObject.SetActive(false);
        battleCamera.gameObject.SetActive(true);
        battleUI.SetActive(true);

        UpdateUI();
    }

    private void EndBattle()
    {
        Debug.Log("Battle ends!");

        battleCamera.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
        battleUI.SetActive(false);

        // Additional logic to clean up after the battle can be added here
    }

    private void OnAttack()
    {
        if (!isPlayerTurn) return;

        Debug.Log("Player attacks!");

        int damage = playerStats.attackPower;
        enemyStats.TakeDamage(damage);
        UpdateUI();

        if (enemyStats.currentHealth <= 0)
        {
            Debug.Log("Enemy defeated!");
            playerStats.GainXP(enemyStats.level * 50); // Award XP based on enemy level
            EndBattle();
        }
        else
        {
            StartCoroutine(EnemyTurn());
        }
    }

    private void OnItem()
    {
        if (!isPlayerTurn) return;

        Debug.Log("Player uses an item!");

        // Implement item logic
        StartCoroutine(EnemyTurn());
    }

    private void OnDefend()
    {
        if (!isPlayerTurn) return;

        Debug.Log("Player defends!");

        // Implement defend logic
        StartCoroutine(EnemyTurn());
    }

    private void OnEscape()
    {
        if (!isPlayerTurn) return;

        Debug.Log("Player tries to escape!");

        bool escaped = Random.value > 0.5f; // 50% chance to escape
        if (escaped)
        {
            Debug.Log("Player escaped!");
            EndBattle();
        }
        else
        {
            Debug.Log("Player failed to escape!");
            StartCoroutine(EnemyTurn());
        }
    }

    private IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy's turn starts");
        isPlayerTurn = false;
        yield return new WaitForSeconds(1f); // Add a delay to simulate enemy thinking time

        Debug.Log("Enemy attacks!");

        int damage = enemyStats.attackPower;
        playerStats.TakeDamage(damage);
        UpdateUI();

        if (playerStats.currentHealth <= 0)
        {
            Debug.Log("Player defeated!");
            // Implement game over logic
            EndBattle();
        }
        else
        {
            isPlayerTurn = true;
        }
        Debug.Log("Enemy's turn ends");
    }

    private void UpdateUI()
    {
        playerHealthText.text = $"{playerStats.characterName} HP: {playerStats.currentHealth}/{playerStats.maxHealth}";
        enemyHealthText.text = $"{enemyStats.characterName} HP: {enemyStats.currentHealth}/{enemyStats.maxHealth}";
    }
}
