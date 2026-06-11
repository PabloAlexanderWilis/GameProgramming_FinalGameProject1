using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMusicManager : MonoBehaviour
{
    public static SceneMusicManager Instance;
    private AudioSource audioSource;

    [Header("Music Settings")]
    public AudioClip sceneMusic;
    public string sceneName;
    public bool loopMusic = true;

    void Awake()
    {
        SceneMusicManager[] managers = FindObjectsOfType<SceneMusicManager>();
        if (managers.Length > 1)
        {
            foreach (SceneMusicManager manager in managers)
            {
                if (manager != this)
                {
                    Destroy(manager.gameObject);
                }
            }
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = sceneMusic;
        audioSource.loop = loopMusic;
        audioSource.playOnAwake = false;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == sceneName)
        {
            if (audioSource != null && audioSource.clip != null)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }
        else
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    void Start()
    {
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }
}