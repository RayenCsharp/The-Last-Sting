using UnityEngine;
using UnityEngine.UI;

public class MenuManeger : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button quitBtn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (startBtn != null) startBtn.onClick.AddListener(() => LoadGameScene());
        if (quitBtn != null) quitBtn.onClick.AddListener(() => QuitGame());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadGameScene()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
