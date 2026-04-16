using TMPro;
using UnityEngine;

public class SnakeUI : MonoBehaviour
{
    public TMP_Text scoreText;

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }
}