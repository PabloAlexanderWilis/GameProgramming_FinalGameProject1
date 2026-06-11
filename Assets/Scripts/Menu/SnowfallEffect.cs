using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SnowfallEffect : MonoBehaviour
{
    [Header("Snowflake Settings")]
    public GameObject snowflakePrefab;
    public int snowflakeCount = 100;
    public float spawnWidth = 2000f;
    public float spawnHeight = 500f;

    [Header("Movement Settings")]
    public float minFallSpeed = 20f;
    public float maxFallSpeed = 40f;
    public float minHorizontalDrift = -5f;
    public float maxHorizontalDrift = 5f;
    public float minSize = 2f;
    public float maxSize = 8f;

    [Header("Depth Settings")]
    public float frontLayerAlpha = 0.9f;
    public float middleLayerAlpha = 0.6f;
    public float backLayerAlpha = 0.3f;

    private List<GameObject> snowflakes = new List<GameObject>();
    private List<RectTransform> snowflakeTransforms = new List<RectTransform>();
    private List<float> fallSpeeds = new List<float>();
    private List<float> horizontalDrifts = new List<float>();

    void Start()
    {
        if (snowflakePrefab != null)
        {
            CreateSnowflakes();
        }
        else
        {
            CreateDefaultSnowflakes();
        }
    }

    void CreateSnowflakes()
    {
        for (int i = 0; i < snowflakeCount; i++)
        {
            GameObject snowflake = Instantiate(snowflakePrefab, transform);
            RectTransform rt = snowflake.GetComponent<RectTransform>();

            float xPos = Random.Range(-spawnWidth / 2, spawnWidth / 2);
            float yPos = Random.Range(0, spawnHeight);
            rt.anchoredPosition = new Vector2(xPos, yPos);
            float size = Random.Range(minSize, maxSize);
            rt.sizeDelta = new Vector2(size, size);
            fallSpeeds.Add(Random.Range(minFallSpeed, maxFallSpeed));
            horizontalDrifts.Add(Random.Range(minHorizontalDrift, maxHorizontalDrift));
            Image img = snowflake.GetComponent<Image>();
            if (img != null)
            {
                float depth = Random.value;
                if (depth < 0.33f) 
                {
                    Color col = img.color;
                    col.a = backLayerAlpha;
                    img.color = col;
                    fallSpeeds[i] *= 0.7f; 
                }
                else if (depth < 0.66f)
                {
                    Color col = img.color;
                    col.a = middleLayerAlpha;
                    img.color = col;
                    fallSpeeds[i] *= 0.85f;
                }
                else 
                {
                    Color col = img.color;
                    col.a = frontLayerAlpha;
                    img.color = col;
                }
            }

            snowflakes.Add(snowflake);
            snowflakeTransforms.Add(rt);
        }
    }

    void CreateDefaultSnowflakes()
    {
        for (int i = 0; i < snowflakeCount; i++)
        {
            
            GameObject snowflake = new GameObject("Snowflake_" + i);
            snowflake.transform.SetParent(transform);
            Image img = snowflake.AddComponent<Image>();
            img.color = new Color(1, 1, 1, Random.Range(0.3f, 0.9f));
            RectTransform rt = snowflake.GetComponent<RectTransform>();
            float xPos = Random.Range(-spawnWidth / 2, spawnWidth / 2);
            float yPos = Random.Range(0, spawnHeight);
            rt.anchoredPosition = new Vector2(xPos, yPos);
            float size = Random.Range(minSize, maxSize);
            rt.sizeDelta = new Vector2(size, size);
            fallSpeeds.Add(Random.Range(minFallSpeed, maxFallSpeed));
            horizontalDrifts.Add(Random.Range(minHorizontalDrift, maxHorizontalDrift));

            snowflakes.Add(snowflake);
            snowflakeTransforms.Add(rt);
        }
    }

    void Update()
    {
        for (int i = 0; i < snowflakes.Count; i++)
        {
            if (snowflakeTransforms[i] != null)
            {
                Vector2 pos = snowflakeTransforms[i].anchoredPosition;
                pos.y -= fallSpeeds[i] * Time.deltaTime;
                pos.x += horizontalDrifts[i] * Time.deltaTime;

                if (pos.y < -600f) 
                {
                    pos.y = spawnHeight;
                    pos.x = Random.Range(-spawnWidth / 2, spawnWidth / 2);

                    if (Random.value > 0.7f)
                    {
                        horizontalDrifts[i] = Random.Range(minHorizontalDrift, maxHorizontalDrift);
                    }
                }

                if (pos.x < -spawnWidth / 2) pos.x = -spawnWidth / 2;
                if (pos.x > spawnWidth / 2) pos.x = spawnWidth / 2;

                snowflakeTransforms[i].anchoredPosition = pos;
            }
        }
    }
}