using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoremanagerScene2 : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // UI สำหรับแสดงคะแนน
    public TextMeshProUGUI finalScoreText; // UI สำหรับแสดงคะแนนสุดท้าย
    private int score = 0; // คะแนนในซีนนี้
    private int finalScore = 0; // คะแนนสุดท้ายในซีนนี้
    
    // ฟังก์ชันสำหรับเพิ่มคะแนน
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    // ฟังก์ชันสำหรับคำนวณคะแนนสุดท้าย
    public void CalculateFinalScore()
    {
        if (GameSettings.difficultyLevel == 0) // Easy Mode
        {
            if (score >= 2200)
                finalScore = 10;
            else if (score >= 2000 && score < 2200)
                finalScore = 9;
            else if (score >= 1500 && score < 2000)
                finalScore = 5;
            else if (score >= 750 && score < 1500)
                finalScore = 3;
            else
                finalScore = 1;
        }
        else if (GameSettings.difficultyLevel == 1) // Normal Mode
        {
            if (score >= 2400)
                finalScore = 10;
            else if (score >= 2200 && score < 2400)
                finalScore = 9;
            else if (score >= 1800 && score < 2200)
                finalScore = 5;
            else if (score >= 1000 && score < 1800)
                finalScore = 3;
            else
                finalScore = 1;
        }
        else if (GameSettings.difficultyLevel == 2) // Hard Mode
        {
            if (score >= 2000)
                finalScore = 10;
            else if (score >= 1800 && score < 2000)
                finalScore = 9;
            else if (score >= 1300 && score < 1800)
                finalScore = 5;
            else if (score >= 500 && score < 1300)
                finalScore = 3;
            else
                finalScore = 1;
        }

        // แสดงผลคะแนนสุดท้ายใน UI
        UpdateFinalScoreText();

        // บันทึกคะแนนสำหรับซีนที่ 2
        Debug.Log("Setting score for Scene 2 in ScoreManager: " + finalScore);
        ScoreManager.Instance.SetScoreForScene(2, finalScore); // บันทึกคะแนนสำหรับซีนที่ 2

        // เรียกฟังก์ชันดรอปจิ๊กซอว์ถ้าได้คะแนน >= 8
        DropJigsawPieceIfEligible();
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

    // ฟังก์ชันเรียกเมื่อเกมจบ
    public void EndGame()
    {
        CalculateFinalScore();  // คำนวณคะแนนสุดท้าย
        Debug.Log("Final score calculated for Scene 2: " + finalScore);
    }

    // ฟังก์ชันตรวจสอบการดรอปจิ๊กซอว์
    private void DropJigsawPieceIfEligible()
    {
        // ตรวจสอบว่าผู้เล่นทำคะแนน 8 ขึ้นไปหรือไม่
        if (finalScore >= 8)
        {
            int difficultyLevel = GameSettings.difficultyLevel;

            // ดรอปจิ๊กซอว์ชิ้นที่ตรงกับระดับความยากและซีนที่เล่น
            JigsawManager.Instance.CollectJigsawPiece(difficultyLevel, 1); // ดรอปชิ้นส่วนซีนที่ 2

            Debug.Log($"Jigsaw piece dropped: Scene 2, Difficulty Level: {difficultyLevel}, Image Part: 2, Final Score: {finalScore}");
        }
        else
        {
            Debug.Log("No jigsaw piece dropped in Scene 2. Final score below threshold.");
        }
    }
}