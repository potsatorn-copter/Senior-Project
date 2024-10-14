using UnityEngine;

public class PerspectiveCameraAdjuster : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        AdjustFOV();
    }

    void AdjustFOV()
    {
        // อัตราส่วนหน้าจอเป้าหมาย (เช่น 16:9)
        float targetAspect = 16f / 9f;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleFactor = windowAspect / targetAspect;

        // ปรับ FOV ให้เหมาะสมกับขนาดหน้าจอ
        if (scaleFactor < 1.0f)
        {
            mainCamera.fieldOfView = mainCamera.fieldOfView / scaleFactor;
        }
    }
}