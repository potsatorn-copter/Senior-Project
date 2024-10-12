using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager1 : MonoBehaviour
{
    public Text scoreText; // สำหรับแสดงจำนวนไอเท็มดีที่เก็บได้
    public TextMeshProUGUI finalScoreText; // สำหรับแสดงผลคะแนนสุดท้าย
    public GameObject endGamePanel; // สำหรับแผง UI ของหน้าจอจบเกม
    private int goodItemCount = 0; // นับจำนวนไอเท็มดีที่เก็บได้
    private int finalScore = 0; // คะแนนสุดท้าย

    private void Start()
    {
        UpdateScoreText(); // อัพเดท UI เมื่อเริ่มเกม
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(false); // ซ่อนแผงจบเกมเมื่อเริ่มเกม
        }
    }

    // ฟังก์ชันเพิ่มจำนวนไอเท็มดีที่เก็บได้
    public void CollectGoodItem()
    {
        goodItemCount++; // เพิ่มตัวนับเมื่อเก็บไอเท็มดี
        UpdateScoreText();
    }

    // ฟังก์ชันลดจำนวนไอเท็มเมื่อเก็บไอเท็มไม่ดี
    public void CollectBadItem()
    {
        goodItemCount--; // ลดตัวนับเมื่อเก็บไอเท็มไม่ดี
        UpdateScoreText();
    }

    // ฟังก์ชันคำนวณคะแนนสุดท้าย
    public void CalculateFinalScore()
    {
        if (goodItemCount >= 11)
        {
            finalScore = 10; // ได้ 11-12 ชิ้น ได้ 10 คะแนน
        }
        else if (goodItemCount >= 9)
        {
            finalScore = 8; // ได้ 9-10 ชิ้น ได้ 8 คะแนน
        }
        else if (goodItemCount >= 6)
        {
            finalScore = 6; // ได้ 6-8 ชิ้น ได้ 6 คะแนน
        }
        else if (goodItemCount >= 3)
        {
            finalScore = 4; // ได้ 3-5 ชิ้น ได้ 4 คะแนน
        }
        else if (goodItemCount >= 1)
        {
            finalScore = 2; // ได้ 1-2 ชิ้น ได้ 2 คะแนน
        }
        else
        {
            finalScore = 0; // ถ้าไม่ได้เก็บไอเท็มดีเลย ได้ 0 คะแนน
        }

        // แสดงผลคะแนนสุดท้ายใน UI
        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + finalScore;
        }

        // ซ่อนข้อความคะแนนที่เก็บได้ก่อนหน้าหลังจากเกมจบ
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(false); // ปิดข้อความคะแนน
        }

        // แสดงแผงจบเกม
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(true); // แสดงแผงจบเกมเมื่อคำนวณคะแนนเสร็จ
        }
    }

    // อัพเดท UI เพื่อแสดงจำนวนไอเท็มดีที่เก็บได้
    private void UpdateScoreText()
    {
        if (scoreText != null) // ตรวจสอบว่ามีการอ้างอิงถึง Text ถูกต้อง
        {
            scoreText.text = "Your Score: " + goodItemCount; // แสดงจำนวนไอเท็มดีที่เก็บได้
        }
    }

    // ฟังก์ชันนี้ใช้เพื่อเรียกคะแนนปัจจุบันได้
    public int GetCurrentGoodItemCount()
    {
        return goodItemCount;
    }
}