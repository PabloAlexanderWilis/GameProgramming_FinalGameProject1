using UnityEngine;
using System.Collections;

public class CheckpointBox : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    public bool saveOnTouch = false;
    public bool saveOnProjectileHit = true;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && saveOnTouch)
        {
            UniversalSaveManager.AutoSaveState(); 
            StartCoroutine(CheckpointEffect());
        }
        else if (other.CompareTag("Bullet") && saveOnProjectileHit)
        {
            UniversalSaveManager.AutoSaveState(); 
            StartCoroutine(CheckpointEffect());
            Destroy(other.gameObject);
        }
    }

    IEnumerator CheckpointEffect()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.yellow;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = originalColor;
    }
}