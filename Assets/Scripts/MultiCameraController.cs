using UnityEngine;

public class MultiCameraController : MonoBehaviour
{
    public Camera[] cameras;

    void Start()
    {
        cameras = FindObjectsOfType<Camera>();
        ActivateCamera(0);
    }

    void Update()
    {
        CheckCameraSwitch();
    }

    void CheckCameraSwitch()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            Camera cam = cameras[i];
            Vector3 viewportPos = cam.WorldToViewportPoint(Player.Instance.transform.position);

            if (viewportPos.x >= 0 && viewportPos.x <= 1 &&
                viewportPos.y >= 0 && viewportPos.y <= 1 &&
                viewportPos.z > 0)
            {
                if (cam.gameObject.activeSelf == false)
                {
                    ActivateCamera(i);
                    break;
                }
            }
        }
    }

    void ActivateCamera(int index)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == index);
        }
    }
}