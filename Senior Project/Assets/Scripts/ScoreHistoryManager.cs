using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Threading.Tasks;
using TMPro;

[Serializable]
public class QuizResult
{
    public int score;
    public string date;
}

[Serializable]
public class QuizHistory
{
    public List<QuizResult> results = new List<QuizResult>();
}

public class ScoreHistoryManager : MonoBehaviour
{
    private string filePath;
    private QuizHistory quizHistory;
    public TextMeshProUGUI scoreHistoryText; // อ้างอิงถึง TextMeshPro ที่จะใช้แสดงคะแนน

    void  Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "quizHistory.json");
        Debug.Log("ตำแหน่งไฟล์ JSON: " + filePath);
        LoadHistoryAsync(); // ใช้ฟังก์ชัน async เพื่อโหลดประวัติ
    }

    public async void SaveScore(int score)
    {
        if (quizHistory == null)
        {
            quizHistory = new QuizHistory(); // กำหนดค่า quizHistory ถ้ายังไม่ได้ถูกสร้าง
            Debug.Log("สร้างออบเจ็ค QuizHistory ใหม่");
        }

        if (score > 0)
        {
            QuizResult newResult = new QuizResult
            {
                score = score,
                date = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
            };

            quizHistory.results.Add(newResult);
            Debug.Log("เพิ่มคะแนนใหม่ในประวัติ: Score = " + newResult.score + " Date = " + newResult.date);

            await SaveHistoryAsync(); // รอให้ข้อมูลบันทึกเสร็จ
            DisplayScoreHistory();    // แสดงข้อมูลหลังจากบันทึกเสร็จ
        }
    }
    

    private async Task SaveHistoryAsync()
    {
        try
        {
            string json = JsonUtility.ToJson(quizHistory, true);
            Debug.Log("ข้อมูลที่กำลังจะถูกบันทึกลง JSON: " + json);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                await writer.WriteAsync(json); // บันทึกไฟล์แบบ async
            }
            Debug.Log("บันทึกไฟล์ JSON สำเร็จที่ตำแหน่ง: " + filePath);
        }
        catch (Exception e)
        {
            Debug.LogError("เกิดข้อผิดพลาดในการบันทึกไฟล์ JSON: " + e.Message);
        }
    }

    private async Task LoadHistoryAsync()
    {
        if (File.Exists(filePath))
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string json = await reader.ReadToEndAsync(); // โหลดไฟล์แบบ async
                    quizHistory = JsonUtility.FromJson<QuizHistory>(json);
                    Debug.Log("ข้อมูลที่โหลดจากไฟล์ JSON: " + json);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("เกิดข้อผิดพลาดในการโหลดไฟล์ JSON: " + e.Message);
                quizHistory = new QuizHistory(); // สร้างใหม่หากมีปัญหา
            }
        }
        else
        {
            quizHistory = new QuizHistory(); // สร้างใหม่หากไม่มีไฟล์
            Debug.LogWarning("ไม่มีไฟล์ประวัติที่บันทึกไว้");
        }

        DisplayScoreHistory(); // เรียกแสดงข้อมูลหลังโหลดประวัติ
    }
    
    // ฟังก์ชันแสดงประวัติการบันทึก
    public void DisplayScoreHistory()
    {
        if (quizHistory != null && quizHistory.results.Count > 0)
        {
            string history = "Score History:\n";
            foreach (var result in quizHistory.results)
            {
                history += $"Score: {result.score} | Date: {result.date}\n"; // แสดงเฉพาะสกอร์รวมที่บันทึกแล้ว
            }

            // อัปเดตข้อความใน TextMeshProUGUI
            scoreHistoryText.SetText(history);
        }
        else
        {
            scoreHistoryText.SetText("No history available.");
        }
    }

    public void ClearScoreHistory()
    {
        quizHistory.results.Clear(); // ล้างข้อมูลในประวัติทั้งหมด

        SaveHistoryAsync(); // บันทึกข้อมูลที่ถูกล้างลงไฟล์ JSON

        DisplayScoreHistory(); // อัปเดต UI
    }
}
