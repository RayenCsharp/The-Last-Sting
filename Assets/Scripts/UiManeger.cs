using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManeger : MonoBehaviour
{

    [Header("Refrences")]
    [SerializeField] private GameManeger gameManeger;
    [Header("UI Pause Elements")]
    [SerializeField] private GameObject gamePauseUI;
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button QuitBtn;

    [Header("UI Game Over Elements")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button MenuBtn;

    [Header("UI Game Elements")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider poisenBar;
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private TextMeshProUGUI hpTxt;
    [SerializeField] private TextMeshProUGUI PoisenTxt;
    [SerializeField] private GameObject bossHpUi;
    [SerializeField] private Slider bossHp;

    [Header("UI Tutorial Elements")]
    [SerializeField] private GameObject basicTutorialUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gamePauseUI.SetActive(false);
        gameOverUI.SetActive(false);
        basicTutorialUI.SetActive(true);
        bossHpUi.SetActive(false);
        Invoke("HideTutorial", 5f);

        if (restartBtn != null) restartBtn.onClick.AddListener(() => gameManeger.RestartGame());
        if (resumeBtn != null) resumeBtn.onClick.AddListener(() => gameManeger.ResumeGame());
        if (QuitBtn != null) QuitBtn.onClick.AddListener(() => gameManeger.MenuScene());
        if (MenuBtn != null) MenuBtn.onClick.AddListener(() => gameManeger.MenuScene());
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void UpdateHealth (float currHealth,  float MaxHealth)
    {
        healthBar.value = currHealth / MaxHealth;
        hpTxt.text = currHealth.ToString();
    }

    public void UpdatePoisen(float currPoisen, float MaxPoisen)
    {
        poisenBar.value = currPoisen / MaxPoisen;
        PoisenTxt.text = currPoisen.ToString();
    }

    public void UpdateScore(int score)
    {
        scoreTxt.text = "Score: " + score;
    }

    public void ShowGameOver()
    {
        gameOverUI.SetActive(true);
    }

    public void TogglePauseMenu(bool isPaused)
    {
        gamePauseUI.SetActive(isPaused);
    }

    private void HideTutorial()
    {
        basicTutorialUI.SetActive(false);
    }

    public void BossHp(float currHp, float MaxHp)
    {
        bossHpUi.SetActive(true);
        bossHp.value = currHp / MaxHp;
    }
}
