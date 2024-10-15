using System.Collections;
using System.Collections.Generic;
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

    // กำหนดช่วงคะแนนสำหรับแต่ละระดับความยาก
    private Dictionary<int, int[]> difficultyThresholds = new Dictionary<int, int[]>()
    {
        { 0, new int[] { 11, 9, 6, 3, 1 } }, // Easy: [11-12 = 10, 9-10 = 8, 6-8 = 6, 3-5 = 4, 1-2 = 2]
        { 1, new int[] { 14, 12, 9, 5, 1 } }, // Normal: [14-15 = 10, 12-13 = 8, 9-11 = 6, 5-8 = 4, 1-4 = 2]
        { 2, new int[] { 15, 13, 9, 5, 1 } }  // Hard: [15-16 = 10, 13-14 = 8, 9-12 = 6, 5-8 = 4, 1-4 = 2]
    };

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
        int difficulty = GameSettings.difficultyLevel; // ตรวจสอบระดับความยากจาก GameSettings
        int[] thresholds = difficultyThresholds[difficulty]; // นำ threshold ของระดับความยากนั้นมาใช้

        // ใช้ threshold ในการกำหนดคะแนน
        if (goodItemCount >= thresholds[0])
            finalScore = 10;
        else if (goodItemCount >= thresholds[1])
            finalScore = 8;
        else if (goodItemCount >= thresholds[2])
            finalScore = 6;
        else if (goodItemCount >= thresholds[3])
            finalScore = 4;
        else if (goodItemCount >= thresholds[4])
            finalScore = 2;
        else
            finalScore = 0;

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
        Debug.Log("Setting score for Scene 1 in ScoreManager: " + finalScore);
        ScoreManager.Instance.SetScoreForScene(1, finalScore); // บันทึกคะแนนสำหรับซีนที่ 1
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