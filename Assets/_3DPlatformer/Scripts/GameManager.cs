using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [Header("HUD and Panels")]
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public TMP_Text winMessage;
    public TMP_Text finalCoinText;
    public GameObject HUD;


    [Header("Coins")]
    public int coins;
    public int coinsToWin;
    public TMP_Text coinHUDText;

    private void Awake()
    {
        coins = 0;
        if(coinHUDText != null )
        {
            coinHUDText.text = "Coins: " + coins.ToString();
        }
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void pauseGame()
    {
        UnlockCursor();
        HUD.SetActive(false);
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        LockCursor();
        Time.timeScale = 1f;
        HUD.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void Gameover()
    {
        if (gameOverPanel != null)
        {
            UnlockCursor();
            gameOverPanel.SetActive(true);
            finalCoinText.text = "Coins Collected: " + coins.ToString();
            Time.timeScale = 0;
        }
    }
    public void OnWin()
    {
        if (winPanel != null)
            {
                UnlockCursor();
                HUD.SetActive(false);
                winPanel.SetActive(true);
                winMessage.text = "You managed to collect " + coinsToWin.ToString() + " coins and completed the level!";
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
        UnlockCursor();
        SceneManager.LoadScene(0);
    }
    public void toGame()
    {
        LockCursor();
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
            UnlockCursor();
            pauseGame();
        }
    }
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void AddCoin()
    {
        coins++;
        coinHUDText.text = "Coins: " + coins.ToString();
        if (coins >= coinsToWin) {
            OnWin();
        }
    }
}
