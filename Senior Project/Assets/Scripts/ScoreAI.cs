using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAI : MonoBehaviour
{
    public Text  scoreAIText; // อ้างอิงไปยัง TextMeshPro text สำหรับแสดงคะแนน
    private int scoreAI = 0; // ตัวแปรสำหรับเก็บคะแนนปัจจุบัน

    private void Start()
    {
        UpdateScoreTextAI(); // อัพเดท UI เมื่อเริ่มเกม
    }

    public void AddScoreAI(int pointsAdd)
    {
        scoreAI += pointsAdd;
        UpdateScoreTextAI(); // อัพเดท UI ทุกครั้งที่คะแนนเปลี่ยนแปลง
    }

    public void SubtractScoreAI(int pointsSubtract)
    {
        scoreAI -= pointsSubtract;
        UpdateScoreTextAI(); // อัพเดท UI ทุกครั้งที่คะแนนเปลี่ยนแปลง
    }

    private void UpdateScoreTextAI()
    {
        if (scoreAIText != null) // ตรวจสอบว่าอ้างอิงไปยัง TextMeshPro ถูกต้อง
        {
            scoreAIText.text = "Score: " + scoreAI; // อัพเดทข้อความ UI
        }
    }
    public int GetCurrentScoreAI()
    {
        return scoreAI;
    }
}
