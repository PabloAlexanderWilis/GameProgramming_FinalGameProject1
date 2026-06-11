using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Elements")]
    public GameObject pauseText;
    public GameObject gameOverText;

    public bool isPaused = false;
    public bool isGameOver = false;

    void Awake()
    {
        GameManager[] managers = FindObjectsOfType<GameManager>();
        if (managers.Length > 1)
        {
            for (int i = 1; i < managers.Length; i++)
            {
                Destroy(managers[i].gameObject);
            }
        }

        if (Instance == null)
        {
            Instance = this;
        }
        else if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (pauseText != null) pauseText.SetActive(false);
        if (gameOverText != null) gameOverText.SetActive(false);

        isPaused = false;
        isGameOver = false;
        Time.timeScale = 1f;
    }

    public void ResetUIState()
    {
        if (pauseText != null) pauseText.SetActive(false);
        if (gameOverText != null) gameOverText.SetActive(false);

        isPaused = false;
        isGameOver = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        if (pauseText != null) pauseText.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (pauseText != null) pauseText.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ShowGameOver()
    {
        isGameOver = true;
        if (gameOverText != null) gameOverText.SetActive(true);
    }

    public void HideGameOver()
    {
        isGameOver = false;
        if (gameOverText != null) gameOverText.SetActive(false);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManagement.Instance.LoadMainMenu();
    }
}