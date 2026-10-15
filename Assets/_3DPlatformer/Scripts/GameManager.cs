using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    [Header("Panel")]
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public TMP_Text finalCoinText;
    public int coins;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void pauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void Gameover()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            finalCoinText.text = "Coins Collected: " + coins.ToString();
            Time.timeScale = 0;
        }
    }
    public void RestartGame()
    {
        UnpauseGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void toTitleScreen()
    {
        UnpauseGame();
        SceneManager.LoadScene(0);
    }
    public void toGame()
    {
        SceneManager.LoadScene(1);
    }
    public void exitGame()
    {
        Application.Quit();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed && pausePanel !=null)
        {
            pauseGame();
        }
    }
}
