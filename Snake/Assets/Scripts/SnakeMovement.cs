using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SnakeMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveStep = 1f;
    public float moveDelay = 0.2f;
    public float minMoveDelay = 0.05f;

    [Header("Speed Ramp")]
    public int foodPerSpeedIncrease = 5;
    public float speedIncreaseAmount = 0.02f;

    [Header("Body")]
    public GameObject bodyPrefab;

    [Header("Food Spawn Range")]
    public int minX = -8;
    public int maxX = 8;
    public int minY = -4;
    public int maxY = 4;

    [Header("UI")]
    public SnakeUI uiManager;
    public GameObject losePanel;
    public TextMeshProUGUI finalScoreText;

    private Vector2 direction = Vector2.right;
    private float timer;

    private readonly List<Transform> bodyParts = new List<Transform>();
    private readonly List<Vector3> previousPositions = new List<Vector3>();

    private bool isGameOver = false;
    private bool shouldGrow = false;

    private int foodCount = 0;

    void Start()
    {
        if (uiManager != null)
        {
            uiManager.UpdateScore(foodCount);
        }

        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }
    }

    void Update()
    {
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GoToMenu();
            }

            return;
        }

        HandleInput();

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Move();
            timer = moveDelay;
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && direction != Vector2.down)
            direction = Vector2.up;

        if (Input.GetKeyDown(KeyCode.DownArrow) && direction != Vector2.up)
            direction = Vector2.down;

        if (Input.GetKeyDown(KeyCode.LeftArrow) && direction != Vector2.right)
            direction = Vector2.left;

        if (Input.GetKeyDown(KeyCode.RightArrow) && direction != Vector2.left)
            direction = Vector2.right;
    }

    void Move()
    {
        previousPositions.Clear();

        previousPositions.Add(transform.position);

        for (int i = 0; i < bodyParts.Count; i++)
        {
            previousPositions.Add(bodyParts[i].position);
        }

        transform.position += (Vector3)(direction * moveStep);
        WrapAround();

        for (int i = 0; i < bodyParts.Count; i++)
        {
            bodyParts[i].position = previousPositions[i];
        }

        if (shouldGrow)
        {
            Vector3 spawnPosition = previousPositions[previousPositions.Count - 1];
            GameObject newPart = Instantiate(bodyPrefab, spawnPosition, Quaternion.identity);
            bodyParts.Add(newPart.transform);
            shouldGrow = false;
        }

        CheckSelfCollision();
    }

    void WrapAround()
    {
        Vector3 newPosition = transform.position;

        if (newPosition.x < minX)
            newPosition.x = maxX;
        else if (newPosition.x > maxX)
            newPosition.x = minX;

        if (newPosition.y < minY)
            newPosition.y = maxY;
        else if (newPosition.y > maxY)
            newPosition.y = minY;

        transform.position = newPosition;
    }

    void CheckSelfCollision()
    {
        if (bodyParts.Count < 3) return;

        for (int i = 0; i < bodyParts.Count; i++)
        {
            if (Vector3.Distance(transform.position, bodyParts[i].position) < 0.01f)
            {
                isGameOver = true;

                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayGameOver();
                }

                if (losePanel != null)
                {
                    losePanel.SetActive(true);
                }

                if (finalScoreText != null)
                {
                    finalScoreText.text = "Score: " + foodCount;
                }

                Debug.Log("Game Over - Press R to Restart / ESC for Menu");
                return;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isGameOver) return;

        if (other.CompareTag("Food"))
        {
            shouldGrow = true;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayEat();
            }

            if (ParticleManager.Instance != null)
            {
                ParticleManager.Instance.PlayFoodEffect(other.transform.position);
            }

            foodCount++;

            if (uiManager != null)
            {
                uiManager.UpdateScore(foodCount);
            }

            if (foodCount % foodPerSpeedIncrease == 0)
            {
                moveDelay -= speedIncreaseAmount;

                if (moveDelay < minMoveDelay)
                    moveDelay = minMoveDelay;

                Debug.Log("Speed Increased: " + moveDelay);
            }

            MoveFoodToSafePosition(other.transform);
        }
    }

    void MoveFoodToSafePosition(Transform foodTransform)
    {
        for (int attempt = 0; attempt < 100; attempt++)
        {
            Vector3 newPosition = new Vector3(
                Random.Range(minX, maxX + 1),
                Random.Range(minY, maxY + 1),
                0f
            );

            if (PositionIsFree(newPosition))
            {
                foodTransform.position = newPosition;
                return;
            }
        }

        Debug.LogWarning("No free position found for food.");
    }

    bool PositionIsFree(Vector3 position)
    {
        if (Vector3.Distance(transform.position, position) < 0.01f)
            return false;

        for (int i = 0; i < bodyParts.Count; i++)
        {
            if (Vector3.Distance(bodyParts[i].position, position) < 0.01f)
                return false;
        }

        return true;
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}