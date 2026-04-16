using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Score UI")]
    public TextMeshProUGUI leftScoreText;
    public TextMeshProUGUI rightScoreText;
    public TextMeshProUGUI scorePopupText;

    [Header("Win UI")]
    public GameObject winPanel;
    public TextMeshProUGUI winnerText;

    [Header("Game References")]
    public BallController ball;

    [Header("Win Condition")]
    public int winningScore = 10;

    private int leftScore = 0;
    private int rightScore = 0;
    private bool gameEnded = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateScoreUI();

        if (scorePopupText != null)
        {
            scorePopupText.gameObject.SetActive(false);
        }

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (!gameEnded) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void ScoreLeft()
    {
        if (gameEnded) return;

        leftScore++;
        UpdateScoreUI();

        if (leftScore >= winningScore)
        {
            EndGame("Left Player Wins!");
        }
        else
        {
            StartCoroutine(ShowScoreAndReset("Left Player Scores!"));
        }
    }

    public void ScoreRight()
    {
        if (gameEnded) return;

        rightScore++;
        UpdateScoreUI();

        if (rightScore >= winningScore)
        {
            EndGame("Right Player Wins!");
        }
        else
        {
            StartCoroutine(ShowScoreAndReset("Right Player Scores!"));
        }
    }

    void EndGame(string message)
    {
        gameEnded = true;

        if (ball != null)
        {
            ball.StopBall();
        }

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        if (winnerText != null)
        {
            winnerText.text = message;
        }
    }

    void UpdateScoreUI()
    {
        if (leftScoreText != null)
        {
            leftScoreText.text = leftScore.ToString();
        }

        if (rightScoreText != null)
        {
            rightScoreText.text = rightScore.ToString();
        }
    }

    IEnumerator ShowScoreAndReset(string message)
    {
        if (scorePopupText != null)
        {
            scorePopupText.text = message;
            scorePopupText.gameObject.SetActive(true);
        }

        if (ball != null)
        {
            ball.StopBall();
        }

        yield return new WaitForSeconds(1f);

        if (scorePopupText != null)
        {
            scorePopupText.gameObject.SetActive(false);
        }

        if (!gameEnded && ball != null)
        {
            ball.ResetBall();
        }
    }
}