using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScene : MonoBehaviour
{
    [Header("Scene Settings")]
    public string mainMenuScene = "MainMenu";

    void Update()
    {
        if (Input.GetMouseButtonDown(0) ||
            Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            GoToMainMenu();
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            GoToMainMenu();
        }
    }

    void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}