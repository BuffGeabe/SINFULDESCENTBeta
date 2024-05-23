using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance; // Singleton instance

    public float moveSpeed = 5f;
    public LayerMask obstacleLayer;
    public float encounterChance = 0.1f; // 10% chance of an encounter

    public Camera worldCamera;
    public Camera battleCamera;
    public GameObject battleUI;
    public BattleSystem battleSystem;

    public CharacterStats playerStats;
    public CharacterStats enemyPrefab; // Prefab for enemy character

    public Text currencyText; // Reference to the currency text UI

    private Vector2 targetPosition;
    private bool isMoving;
    private Vector2 movementDirection;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Ensure this gameObject persists
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Destroy duplicate player instances
            return;
        }
    }

    void Start()
    {
        targetPosition = transform.position;
        battleCamera.gameObject.SetActive(false);
        battleUI.SetActive(false);
        UpdateCurrencyUI(); // Initialize the currency UI
    }

    void Update()
    {
        if (!isMoving)
        {
            HandleInput();
        }

        if (movementDirection != Vector2.zero)
        {
            Move();
        }
    }

    private void HandleInput()
    {
        movementDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            movementDirection = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            movementDirection = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementDirection = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            movementDirection = Vector2.right;
        }

        if (movementDirection != Vector2.zero)
        {
            Vector2 newPosition = (Vector2)transform.position + movementDirection;
            if (!Physics2D.OverlapCircle(newPosition, 0.1f, obstacleLayer))
            {
                targetPosition = newPosition;
            }
            else
            {
                movementDirection = Vector2.zero; // Stop movement if an obstacle is detected
            }
        }
    }

    private void Move()
    {
        isMoving = true;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if ((Vector2)transform.position == targetPosition)
        {
            isMoving = false;
            CheckForEncounter();
        }
    }

    private void CheckForEncounter()
    {
        float randomValue = Random.Range(0f, 1f);
        if (randomValue < encounterChance)
        {
            CharacterStats enemyInstance = Instantiate(enemyPrefab);
            enemyInstance.level = Random.Range(1, 5); // Example: Random level between 1 and 4
            battleSystem.TriggerBattle(playerStats, enemyInstance);
        }
    }

    public void UpdateCurrencyUI()
    {
        currencyText.text = $"Currency: {playerStats.currency}";
    }
}
