using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // อย่าลืม import สำหรับ TextMeshPro

public class ScoreManager1 : MonoBehaviour
{
    public Text  scoreText; // อ้างอิงไปยัง TextMeshPro text สำหรับแสดงคะแนน
    private int score = 0; // ตัวแปรสำหรับเก็บคะแนนปัจจุบัน

    private void Start()
    {
        UpdateScoreText(); // อัพเดท UI เมื่อเริ่มเกม
    }

    public void AddScore(int pointsToAdd)
    {
        score += pointsToAdd;
        UpdateScoreText(); // อัพเดท UI ทุกครั้งที่คะแนนเปลี่ยนแปลง
    }

    public void SubtractScore(int pointsToSubtract)
    {
        score -= pointsToSubtract;
        UpdateScoreText(); // อัพเดท UI ทุกครั้งที่คะแนนเปลี่ยนแปลง
    }

    private void UpdateScoreText()
    {
        if (scoreText != null) // ตรวจสอบว่าอ้างอิงไปยัง TextMeshPro ถูกต้อง
        {
            scoreText.text = "Score: " + score; // อัพเดทข้อความ UI
        }
    }
}
