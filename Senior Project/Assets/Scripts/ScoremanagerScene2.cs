using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoremanagerScene2 : MonoBehaviour
{
    public static ScoremanagerScene2 Instance;
    public TextMeshProUGUI scoreText; // UI สำหรับแสดงคะแนน
    public TextMeshProUGUI finalScoreText; // UI สำหรับแสดงคะแนนสุดท้าย
    private int score = 0;
    private int finalScore = 0;
    public GameObject gameOverUI; // UI ที่จะโชว์เมื่อเกมจบ

    private void Awake()
    {
        // สร้าง Singleton สำหรับ ScoreManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ฟังก์ชันสำหรับเพิ่มคะแนน
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
        Debug.Log(score);
    }

    // ฟังก์ชันสำหรับคำนวณคะแนนสุดท้าย
    public void CalculateFinalScore()
    {
        if (score >= 2200)
        {
            finalScore = 10;
        }
        else if (score >= 2000 && score < 2100)
        {
            finalScore = 9;
        }
        else if (score >= 1500)
        {
            finalScore = 5;
        }
        else if (score >= 750)
        {
            finalScore = 3;
        }
        else if (score < 750)
        {
            finalScore = 1;
        }
    }

    // อัปเดตการแสดงคะแนนบน UI
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    // อัปเดตการแสดงคะแนนสุดท้ายบน UI
    private void UpdateFinalScoreText()
    {
        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + finalScore;
        }
    }
    // เรียกฟังก์ชันนี้เมื่อเกมจบ
    // ในฟังก์ชัน EndGame ให้แน่ใจว่า CalculateFinalScore ถูกเรียกหลังจากที่คะแนนถูกอัปเดต
    public void EndGame()
    {
        // เพิ่มการอัปเดตคะแนนก่อนการคำนวณคะแนนสุดท้าย
        Debug.Log("Ending game with score: " + score);

        CalculateFinalScore();  // คำนวณคะแนนสุดท้าย
        Debug.Log("Final score calculated: " + finalScore);  // ตรวจสอบว่าได้เรียกฟังก์ชันแล้ว
        UpdateFinalScoreText(); // อัปเดตการแสดงผล
    }
}
