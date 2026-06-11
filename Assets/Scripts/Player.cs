using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [System.Serializable]
    public struct PlayerState
    {
        public Vector3 position;
        public int extraJumpsFromPellets;
        public int totalDeaths;
    }

    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public int extraJumpValue = 1;
    private int extraJumps;
    public int extraJumpsFromPellets = 0;

    public static int totalDeaths = 0;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    [Header("Sound Effects")]
    public AudioClip jumpSound;
    public AudioClip shootSound;
    public AudioClip deathSound;
    public AudioClip pelletCollectSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        extraJumps = extraJumpValue;

        // Create audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 0.7f;

        // ALWAYS RE-ESTABLISH SINGLETON
        Instance = this;

        // RESET UI STATE
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetUIState();
        }
    }

    public static Player Instance;

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput > 0) spriteRenderer.flipX = false;
        else if (moveInput < 0) spriteRenderer.flipX = true;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                extraJumps = extraJumpValue;
                extraJumpsFromPellets = 0;
                PlaySound(jumpSound);
            }
            else if (extraJumps > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                extraJumps--;
                PlaySound(jumpSound);
            }
            else if (extraJumpsFromPellets > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                extraJumpsFromPellets--;
                PlaySound(jumpSound);
            }
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            if (UniversalSaveManager.IsEmpty())
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(
                    UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }
            else
            {
                UniversalSaveManager.AutoLoadState();

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ResetUIState();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            ShootProjectile();
            PlaySound(shootSound);
        }

        SetAnimation(moveInput);

        if (transform.position.y < -10)
        {
            Die();
        }
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    public GameObject projectilePrefab;
    public Transform shootPoint;

    void ShootProjectile()
    {
        if (projectilePrefab != null && shootPoint != null)
        {
            GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

            Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();

            if (projRb != null)
            {
                if (spriteRenderer.flipX)
                {
                    projRb.velocity = new Vector2(-10, 0);
                }
                else
                {
                    projRb.velocity = new Vector2(10, 0);
                }
            }
            else
            {
                Debug.LogError("Projectile prefab missing Rigidbody2D!");
            }
        }
        else
        {
            if (projectilePrefab == null) Debug.LogError("Projectile Prefab is null!");
            if (shootPoint == null) Debug.LogError("Shoot Point is null!");
        }
    }

    void SetAnimation(float moveInput)
    {
        if (IsGrounded())
        {
            if (moveInput == 0) animator.Play("Player_Idle");
            else animator.Play("Player_Run");
        }
        else
        {
            if (rb.velocity.y > 0) animator.Play("Player_Jump");
            else animator.Play("Player_Fall");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard") ||
            collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Damage"))
        {
            Die();
        }
    }

    void Die()
    {
        totalDeaths++;
        PlaySound(deathSound);

        Time.timeScale = 1f;
        SpawnBloodParticles();
        GameManager.Instance.ShowGameOver();
        Destroy(gameObject);
    }

    void SpawnBloodParticles()
    {
        GameObject bloodPrefab = Resources.Load<GameObject>("Blood");

        if (bloodPrefab != null)
        {
            for (int i = 0; i < 129; i++)
            {
                Vector3 randomPos = transform.position + new Vector3(
                    Random.Range(-0.5f, 0.5f),
                    Random.Range(-0.5f, 0.5f),
                    0
                );

                GameObject blood = Instantiate(bloodPrefab, randomPos, Quaternion.identity);

                if (blood.GetComponent<Rigidbody2D>() != null)
                {
                    blood.GetComponent<Rigidbody2D>().velocity = new Vector2(
                        Random.Range(-5f, 5f),
                        Random.Range(-5f, 5f)
                    );
                }
            }
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}