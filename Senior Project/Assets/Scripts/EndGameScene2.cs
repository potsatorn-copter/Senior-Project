using System.Collections;
using UnityEngine;

public class EndGameScene2 : MonoBehaviour
{
    public GameObject uiToShow; // UI ที่จะโชว์เมื่อชนกับไอเท็ม
    public string playerTag = "Player"; // Tag ของ Player เพื่อเช็คการชน
    public float delayBeforeShowingUI = 0.5f; // เวลาที่จะหน่วงก่อนโชว์ UI

    private void Start()
    {
        if (uiToShow != null)
        {
            uiToShow.SetActive(false); // ซ่อน UI ตอนเริ่มต้น
        }
    }

    // เมื่อชนกับไอเท็ม
    private void OnTriggerEnter(Collider other)
    {
        // เช็คว่าผู้เล่นชนกับไอเท็มหรือไม่
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Player hit the item!"); // ตรวจสอบว่าผู้เล่นชนกับไอเท็ม
            ShowUI();
            ScoremanagerScene2.Instance.EndGame(); // คำนวณคะแนนสุดท้าย
        }
    }

    // ฟังก์ชันสำหรับแสดง UI และหยุดเกม
    private void ShowUI()
    {
        if (uiToShow != null)
        {
            Debug.Log("Showing UI"); // ตรวจสอบว่า UI ถูกเรียกมาแสดง
            uiToShow.SetActive(true); // แสดง UI
        }

        Time.timeScale = 0f; // หยุดการทำงานของเกม
    }
}