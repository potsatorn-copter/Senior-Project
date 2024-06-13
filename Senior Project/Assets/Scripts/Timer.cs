using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Timer : MonoBehaviour
{
    [SerializeField] private float startTime = 30.0f; // เวลาเริ่มต้น 30 วินาที
    private float timeCount;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject gameOverUI; // UI ที่จะแสดงเมื่อเวลาจบ
    private bool isTimerRunning = true; // ตัวแปรสำหรับควบคุมการทำงานของตัวนับเวลา

    private void Start()
    {
        timeCount = startTime; // ตั้งค่าเวลานับถอยหลังให้เป็นค่าเริ่มต้น
        UpdateTimeText(); // อัปเดตข้อความเวลาเมื่อเริ่มเกม
    }

    private void Update()
    {
        if (isTimerRunning && timeCount > 0)
        {
            timeCount -= Time.deltaTime; // ลดเวลานับถอยหลังตามเวลาที่ผ่านไปในแต่ละเฟรม
            if (timeCount < 0)
            {
                timeCount = 0; // ป้องกันเวลาติดลบ
                EndTimer();
            }
            UpdateTimeText(); // อัปเดตข้อความเวลา
        }
    }

    private void UpdateTimeText()
    {
        int minutes = Mathf.FloorToInt(timeCount / 60); // คำนวณนาทีจากเวลาที่เหลือ
        int seconds = Mathf.FloorToInt(timeCount % 60); // คำนวณวินาทีจากเวลาที่เหลือ
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // อัปเดตข้อความเวลาที่แสดงผล
    }

    private void EndTimer()
    {
        SoundManager.instance.MuteSound(SoundManager.SoundName.MainmenuSong, true);
        gameOverUI.SetActive(true); // แสดง UI เมื่อเวลาจบ
        Time.timeScale = 0; // หยุดเกม
    }

    public void StopTimer()
    {
        isTimerRunning = false; // หยุดการนับเวลา
    }
}