using UnityEngine;
using UnityEngine.UI;

public class TogglePauseButton : MonoBehaviour
{
    [SerializeField] private Button pauseButton; // ปุ่มที่เราจะใช้ในการเปลี่ยนค่าของ Time.timeScale
    private bool isPaused = false; // ตัวแปรบอกสถานะว่าเกมหยุดหรือไม่

    void Start()
    {
        // ตรวจสอบให้แน่ใจว่าปุ่มถูกตั้งค่า และเพิ่ม Listener ให้กับปุ่ม
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(TogglePause);
        }
    }

    // ฟังก์ชันที่ถูกเรียกเมื่อกดปุ่ม
    public void TogglePause()
    {
        if (isPaused)
        {
            // ถ้าเกมหยุด ให้เล่นต่อ
            Time.timeScale = 1f;
        }
        else
        {
            // ถ้าเกมเล่นอยู่ ให้หยุดเกม
            Time.timeScale = 0f;
        }

        // สลับสถานะการหยุด
        isPaused = !isPaused;
    }
}