using UnityEngine;

public class Extradoublejumppellet : MonoBehaviour
{
    public float respawnTime = 3f;
    private Collider2D pelletCollider;
    private SpriteRenderer spriteRenderer;
    private Vector3 originalPosition;

    void Start()
    {
        pelletCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalPosition = transform.position;

        Debug.Log("Pellet started at: " + transform.position);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Pellet triggered by: " + other.name + " with tag: " + other.tag);

        if (other.CompareTag("Player"))
        {
            Debug.Log("COLLECTED! Adding extra jump to player");

            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.extraJumpsFromPellets++;
                Debug.Log("Player now has " + player.extraJumpsFromPellets + " extra jumps");
            }

            Despawn();
            Debug.Log("Pellet despawned!");

            Invoke("Respawn", respawnTime);
            Debug.Log("Respawn scheduled in " + respawnTime + " seconds");
        }
        else
        {
            Debug.Log("Not a player, ignoring trigger");
        }
    }

    void Despawn()
    {
        if (pelletCollider != null)
        {
            pelletCollider.enabled = false;
            Debug.Log("Collider disabled");
        }
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
            Debug.Log("Sprite renderer disabled");
        }
    }

    void Respawn()
    {
        Debug.Log("Pellet respawning!");

        if (pelletCollider != null)
        {
            pelletCollider.enabled = true;
            Debug.Log("Collider re-enabled");
        }
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            Debug.Log("Sprite renderer re-enabled");
        }

        transform.position = originalPosition;
    }

}