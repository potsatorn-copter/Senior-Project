using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaAdjuster : MonoBehaviour
{
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    void ApplySafeArea()
    {
        // รับค่า Safe Area จากหน้าจอ
        Rect safeArea = Screen.safeArea;

        // สำหรับการจำลองใน Unity Editor
#if UNITY_EDITOR
        // จำลอง Safe Area (แก้ค่าตามความต้องการ)
        safeArea = new Rect(0, 100, Screen.width, Screen.height - 200);
#endif

        // คำนวณ anchorMin และ anchorMax จาก Safe Area
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        // ปรับค่า RectTransform
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;

        Debug.Log($"Safe Area Applied: {safeArea}");
    }
}