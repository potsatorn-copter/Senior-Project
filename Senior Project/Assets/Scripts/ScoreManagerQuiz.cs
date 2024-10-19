using UnityEngine;
using TMPro;

public class ScoreManagerQuiz : MonoBehaviour
{
    private int[] scores = new int[14];  // เก็บคะแนนของแต่ละคำถาม (สมมติว่ามี 14 คำถาม)
    private int totalScore = 0;  // คะแนนรวม
    public TextMeshProUGUI finalScoreText;  // TextMeshPro เพื่อแสดงคะแนนรวม
    public TextMeshProUGUI memoryStatusText; // TextMeshPro เพื่อแสดงระดับความจำของผู้เล่น

    public ScoreHistoryManager scoreHistoryManager;  // อ้างอิงถึง ScoreHistoryManager

    // ฟังก์ชันนี้ถูกเรียกเมื่อผู้ใช้เลือกช้อยในคำถามหนึ่ง
    public void SetScore(int questionIndex, int score)
    {
        scores[questionIndex] = score;  // ตั้งคะแนนของคำถามนั้น
        CalculateTotalScore();  // คำนวณคะแนนรวม
    }

    public void CalculateTotalScore()
    {
        totalScore = 0;  // รีเซ็ตคะแนนรวม
        foreach (int score in scores)
        {
            totalScore += score;  // รวมคะแนนจากแต่ละคำถาม
        }

        Debug.Log("คะแนนรวมที่คำนวณได้: " + totalScore);  // Debug เพื่อดูคะแนนรวม

        // แสดงคะแนนรวมใน TextMeshPro
        finalScoreText.SetText("คะแนนที่ทำได้รอบนี้: " +   totalScore);

        // แสดงข้อความตามช่วงคะแนน
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

    public void FinishQuiz()
    {
        CalculateTotalScore(); // คำนวณคะแนนรวมทั้งหมด

        Debug.Log("Final score attempting to save: " + totalScore);
        scoreHistoryManager.SaveScore(totalScore); // บันทึกคะแนนรวมหลังจากจบรอบ

        Debug.Log("Score saved successfully.");
    }

    // คืนค่า Total Score
    public int GetTotalScore()
    {
        return totalScore;
    }
}