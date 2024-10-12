using UnityEngine;

public class PanelScaler : MonoBehaviour
{
    public RectTransform endGamePanel; // อ้างอิงไปยัง RectTransform ของ Panel
    public float referenceWidth = 1080f; // ความกว้างอ้างอิงสำหรับ iPhone 12 หรือ 7
    public float referenceHeight = 1920f; // ความสูงอ้างอิงสำหรับ iPhone 12 หรือ 7
    public float scaleFactor = 1.1f; // ตัวแปรเพื่อขยายขนาด Panel สำหรับอุปกรณ์ที่มีจอใหญ่กว่า

    void Start()
    {
        AdjustPanelSize();
    }

    void AdjustPanelSize()
    {
        // รับค่า width และ height ของหน้าจอปัจจุบัน
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // คำนวณอัตราส่วนหน้าจอปัจจุบัน
        float aspectRatio = screenWidth / screenHeight;

        // อัตราส่วนหน้าจอที่ใช้สำหรับ iPhone (เช่น iPhone 12, iPhone 7)
        float referenceAspectRatio = referenceWidth / referenceHeight;

        // ตรวจสอบว่าอัตราส่วนหน้าจอใหญ่กว่า iPhone หรือไม่ (เช่น iPad ที่มีอัตราส่วนจอเล็กกว่า iPhone)
        if (aspectRatio < referenceAspectRatio)
        {
            // ขยาย Panel สำหรับหน้าจอที่มีขนาดใหญ่กว่า iPhone
            Vector2 sizeDelta = endGamePanel.sizeDelta;
            sizeDelta.x = referenceWidth * scaleFactor;  // ขยาย Panel ตามตัวแปร scaleFactor
            sizeDelta.y = referenceHeight * scaleFactor; 
            endGamePanel.sizeDelta = sizeDelta;
        }
        else
        {
            // คงขนาดเดิมสำหรับ iPhone 12 หรือ 7
            Vector2 sizeDelta = endGamePanel.sizeDelta;
            sizeDelta.x = referenceWidth;  // ขนาดเดิมสำหรับ iPhone 12 หรือ 7
            sizeDelta.y = referenceHeight;
            endGamePanel.sizeDelta = sizeDelta;
        }
    }
}