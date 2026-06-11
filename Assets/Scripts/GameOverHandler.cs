using UnityEngine;

public class GameOverHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && GameManager.Instance.isGameOver)
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}