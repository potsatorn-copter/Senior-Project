using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameObjectSettings
{
    public Transform target; // GameObject ที่ต้องการปรับตำแหน่ง
    public Vector3 iPadPosition; // ตำแหน่งสำหรับ iPad
    public Vector3 defaultPosition; // ตำแหน่งเริ่มต้น
    public bool adjustOnIpad = false; // ปรับเฉพาะ iPad หรือไม่
}

public class ResponsiveGameObject : MonoBehaviour
{
    public List<GameObjectSettings> gameObjectSettings; // รายการ GameObject และตำแหน่ง
    [SerializeField] private RectTransform canvasRect; // Canvas RectTransform

    private void Start()
    {
        // ตรวจหา Canvas RectTransform
        Canvas parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas != null)
        {
            canvasRect = parentCanvas.GetComponent<RectTransform>();
        }

        if (canvasRect == null)
        {
            Debug.LogError("ไม่พบ RectTransform ของ Canvas!");
            return;
        }

        // เก็บตำแหน่งเริ่มต้นของ GameObjects
        foreach (var settings in gameObjectSettings)
        {
            if (settings.target != null)
            {
                settings.defaultPosition = settings.target.localPosition;
            }
            else
            {
                Debug.LogWarning("GameObject ที่ไม่ได้ตั้งค่าใน List.");
            }
        }

        AdjustGameObjectPositions();
    }

    private void Update()
    {
        // สามารถเรียกอัพเดตตำแหน่งทุกเฟรมได้ในกรณีที่ต้องการ
        // AdjustGameObjectPositions();
    }

    private void AdjustGameObjectPositions()
    {
        // ตรวจสอบว่าขนาด Canvas เข้าข่าย iPad หรือไม่
        bool isIpad = (canvasRect != null && canvasRect.rect.width >= 1900 && canvasRect.rect.height >= 1300);

        foreach (var settings in gameObjectSettings)
        {
            if (settings.target != null)
            {
                // ปรับตำแหน่งของ GameObject
                if (isIpad && settings.adjustOnIpad)
                {
                    settings.target.localPosition = settings.iPadPosition;
                }
                else
                {
                    settings.target.localPosition = settings.defaultPosition;
                }
            }
        }
    }
}
