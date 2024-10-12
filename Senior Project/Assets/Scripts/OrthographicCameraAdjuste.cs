using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthographicCameraAdjuste : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        AdjustOrthographicSize();
    }

    void AdjustOrthographicSize()
    {
        // อัตราส่วนหน้าจอเป้าหมาย (เช่น 16:9)
        float targetAspect = 16f / 9f;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        // ปรับขนาดกล้องให้เหมาะสม
        if (scaleHeight < 1.0f)
        {
            mainCamera.orthographicSize = mainCamera.orthographicSize / scaleHeight;
        }
    }
}