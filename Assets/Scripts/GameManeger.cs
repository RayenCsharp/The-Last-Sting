using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManeger : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private UiManeger uiManager;

    public int score;
    [Header("Game State")]
    public bool isGamePaused;
    private bool isGameOver;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        isGamePaused = false;
        isGameOver = false;
        uiManager.TogglePauseMenu(isGamePaused);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        uiManager.UpdateScore(score);
    }

    public void GameOver ()
    {
        isGameOver = true;
        Invoke("gameover", 2f);
        uiManager.ShowGameOver();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Game Over");
    }

    private void gameover()
    {
        Time.timeScale = 0;
    }

    public void PauseGame()
    {
        if (isGameOver)
        {
            return;
        }
        Time.timeScale = 0;
        isGamePaused = true;
        uiManager.TogglePauseMenu(isGamePaused);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        if (isGameOver)
        {
            return;
        }
        Time.timeScale = 1;
        isGamePaused = false;
        uiManager.TogglePauseMenu(isGamePaused);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void RestartGame()
    {
        Debug.Log("Restart Game");
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MenuScene ()
    {
        Debug.Log("Menu Scene");
        SceneManager.LoadScene("MenuScene");
    }
}
