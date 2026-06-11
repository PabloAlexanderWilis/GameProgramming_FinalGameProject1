using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement Instance;

    [Header("Scene Names")]
    public string mainMenuScene = "MainMenu";
    public string gameScene = "GameScene";

    void Awake()
    {
        SceneManagement[] managers = FindObjectsOfType<SceneManagement>();
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
        else
        {
            if (this != Instance)
            {
                Destroy(gameObject);
                return;
            }
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}