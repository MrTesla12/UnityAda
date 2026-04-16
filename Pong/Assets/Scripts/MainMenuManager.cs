using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartTwoPlayer()
    {
        GameSettings.isAI = false;
        SceneManager.LoadScene("Game");
    }

    public void StartAI()
    {
        GameSettings.isAI = true;
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game"); // works in editor
    }
}