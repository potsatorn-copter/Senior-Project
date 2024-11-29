using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

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
    public TextMeshProUGUI scoreHistoryText;
    public GameObject clearHistoryButton; // ปุ่มเคลียร์ประวัติ
    public GameObject clearHistoryWarningUI; // UI แจ้งเตือนให้เคลียร์ประวัติ

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "quizHistory.json");
        LoadHistoryAsync(); // โหลดประวัติแบบ async
        clearHistoryWarningUI.SetActive(false); // เริ่มต้นปิดการแสดงผล UI เตือน
    }

    public async void SaveScore(int score)
    {
        if (quizHistory == null)
            quizHistory = new QuizHistory();

        // ตรวจสอบว่ามีประวัติเกิน 8 ครั้งหรือไม่
        if (quizHistory.results.Count >= 7)
        {
            if (clearHistoryWarningUI != null)
            {
                clearHistoryWarningUI.SetActive(true); // เปิดใช้งาน UI เตือน
            }
            return; // ออกจากฟังก์ชันโดยไม่บันทึกคะแนนใหม่
        }

        if (score > 0)
        {
            QuizResult newResult = new QuizResult
            {
                score = score,
                date = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
            };

            // แทรกประวัติใหม่ที่ตำแหน่งแรกของรายการ
            quizHistory.results.Insert(0, newResult);

            await SaveHistoryAsync();
            DisplayScoreHistory();
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
                    string json = await reader.ReadToEndAsync();
                    quizHistory = JsonUtility.FromJson<QuizHistory>(json);
                }
            }
            catch
            {
                quizHistory = new QuizHistory();
            }
        }
        else
        {
            quizHistory = new QuizHistory();
        }

        DisplayScoreHistory();
    }

    public void DisplayScoreHistory()
    {
        if (quizHistory != null && quizHistory.results.Count > 0)
        {
            string history = "\n";
            foreach (var result in quizHistory.results)
            {
                history += $"Score: {result.score} | Date: {result.date}\n";
            }

            scoreHistoryText.SetText(history);

            // แสดง UI เตือนให้เคลียร์ประวัติเมื่อประวัติเกิน 8 ครั้ง
            if (quizHistory.results.Count >= 7)
            {
                if (clearHistoryWarningUI != null)
                {
                    clearHistoryWarningUI.SetActive(true); // เปิดใช้งาน UI เตือน
                }
            }
            else
            {
                if (clearHistoryWarningUI != null)
                {
                    clearHistoryWarningUI.SetActive(false); // ปิดการแสดงผล UI เตือน
                }
            }
        }
        else
        {
            scoreHistoryText.SetText("\nไม่มีประวัติ");
            if (clearHistoryWarningUI != null)
            {
                clearHistoryWarningUI.SetActive(false); // ปิดการแสดงผล UI เตือนเมื่อไม่มีประวัติ
            }
        }
    }

    private async Task SaveHistoryAsync()
    {
        try
        {
            string json = JsonUtility.ToJson(quizHistory, true);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                await writer.WriteAsync(json);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save history: " + e.Message);
        }
    }

    public void ClearScoreHistory()
    {
        quizHistory.results.Clear();
        SaveHistoryAsync();
        DisplayScoreHistory();

        // ปิดการแสดงผล UI เตือนหลังจากเคลียร์ประวัติ
        if (clearHistoryWarningUI != null)
        {
            clearHistoryWarningUI.SetActive(false);
        }
    }
}