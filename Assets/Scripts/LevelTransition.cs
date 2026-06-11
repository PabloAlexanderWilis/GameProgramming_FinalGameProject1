using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [Header("Level Settings")]
    public string nextLevelName = "Level2";
    public bool useSceneBuildIndex = false;
    public int nextSceneIndex = 1; 

    [Header("Transition Effect")]
    public bool fadeToBlack = true;
    public float fadeDuration = 1f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadeToBlack)
            {
                StartCoroutine(TransitionToNextLevel());
            }
            else
            {
                LoadNextLevel();
            }
        }
    }

    System.Collections.IEnumerator TransitionToNextLevel()
    {
        yield return new WaitForSeconds(fadeDuration);
        LoadNextLevel();
    }

    void LoadNextLevel()
    {
        if (useSceneBuildIndex)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(nextLevelName);
        }
    }
}