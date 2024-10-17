using UnityEngine;
using TMPro;

public class ScoreManagerQuiz : MonoBehaviour
{
    private int[] scores = new int[14];  // เก็บคะแนนของแต่ละคำถาม (สมมติว่ามี 14 คำถาม)
    private int totalScore = 0;  // คะแนนรวม
    public TextMeshProUGUI finalScoreText;  // TextMeshPro เพื่อแสดงคะแนนรวม
    public TextMeshProUGUI memoryStatusText; // TextMeshPro เพื่อแสดงระดับความจำของผู้เล่น

    // ฟังก์ชันนี้ถูกเรียกเมื่อผู้ใช้เลือกช้อยในคำถามหนึ่ง
    public void SetScore(int questionIndex, int score)
    {
        scores[questionIndex] = score;  // ตั้งคะแนนของคำถามนั้น
        CalculateTotalScore();  // คำนวณคะแนนรวม

        Debug.Log($"Score updated for question {questionIndex}: {score}");
    }

    private void CalculateTotalScore()
    {
        totalScore = 0;  // รีเซ็ตคะแนนรวม
        foreach (int score in scores)
        {
            totalScore += score;  // รวมคะแนนจากแต่ละคำถาม
        }

        Debug.Log("Total Score calculated: " + totalScore);  // Debug เพื่อตรวจสอบคะแนนรวมที่คำนวณได้

        // แสดงคะแนนรวมใน TextMeshPro
        finalScoreText.SetText("Score: " + totalScore);

        // แสดงผลข้อความตามช่วงคะแนน
        if (totalScore >= 14 && totalScore <= 19)
        {
            memoryStatusText.SetText("ท่านมีความจำดีเยี่ยม");
        }
        else if (totalScore >= 20 && totalScore <= 29)
        {
            memoryStatusText.SetText("ความจำดีปานกลาง");
        }
        else if (totalScore >= 30 && totalScore <= 39)
        {
            memoryStatusText.SetText("ความจำของท่านไม่ดีเท่าไหร่");
        }
        else if (totalScore >= 40 && totalScore <= 56)
        {
            memoryStatusText.SetText("ควรไปปรึกษาแพทย์");
        }
        else
        {
            memoryStatusText.SetText(""); // กรณีที่คะแนนนอกช่วงที่ระบุ
        }
    }

    // ฟังก์ชันนี้จะคืนค่า Total Score เมื่อเรียกใช้
    public int GetTotalScore()
    {
        return totalScore;  // คืนค่าคะแนนรวม
    }
}