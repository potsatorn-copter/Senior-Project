using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Analytics;
using Unity.Services.Core;
using System.Collections.Generic;


public class QuestionnaireController : MonoBehaviour
{
    public GameObject[] questions; // Array to hold all the question GameObjects
    public Button nextButton; // ปุ่มสำหรับคำถามถัดไป
    public Button previousButton; // ปุ่มสำหรับคำถามก่อนหน้า
    public ScoreManagerQuiz scoreManager; // อ้างอิงถึง ScoreManager
    public ScoreHistoryManager scoreHistoryManager; // สำหรับบันทึกคะแนน
    private int currentQuestionIndex = 0; // ตำแหน่งคำถามปัจจุบัน

    async void Start()
    {
        ShowQuestion(currentQuestionIndex);

        // เริ่มต้น Unity Services
        try
        {
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Services Initialized");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to initialize Unity Services: " + e.Message);
        }

        // เพิ่ม Listener ให้ปุ่ม
        nextButton.onClick.AddListener(NextQuestion);
        previousButton.onClick.AddListener(PreviousQuestion);
    }

    public void NextQuestion()
    {
        if (currentQuestionIndex < questions.Length - 1)
        {
            questions[currentQuestionIndex].SetActive(false);
            currentQuestionIndex++;
            ShowQuestion(currentQuestionIndex);
        }
        else
        {
            // ซ่อนคำถามสุดท้ายและจบควิซ
            questions[currentQuestionIndex].SetActive(false);
            FinishQuiz();
        }
    }

    public void PreviousQuestion()
    {
        if (currentQuestionIndex > 0)
        {
            questions[currentQuestionIndex].SetActive(false);
            currentQuestionIndex--;
            ShowQuestion(currentQuestionIndex);
        }
    }

    void ShowQuestion(int index)
    {
        questions[index].SetActive(true); // แสดงคำถามตามตำแหน่ง
    }

    public void FinishQuiz()
    {
        // คำนวณคะแนนรวม
        scoreManager.CalculateTotalScore();

        // บันทึกคะแนน
        int finalScore = scoreManager.GetTotalScore();
        scoreHistoryManager.SaveScore(finalScore);

        Debug.Log("Score saved successfully: " + finalScore);

        // ส่ง Analytics
        SendAnalytics(finalScore);
    }

    private void SendAnalytics(int finalScore)
    {
        try
        {
            // รับ Memory Status
            string memoryStatus = GetMemoryStatus(finalScore);

            // สร้าง CustomEvent ตามตำรา
            CustomEvent analyticsEvent = new CustomEvent("MMSBSCORE")
            {
                { "MmsbScoreString", $"ได้คะแนน {finalScore}/56" },
                { "MemoryStatus", memoryStatus }
            };

            // ส่ง Event ผ่าน AnalyticsService
            AnalyticsService.Instance.RecordEvent(analyticsEvent);

            Debug.Log("Analytics event MMSBSCORE sent successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to send Analytics event: " + e.Message);
        }
    }

    private string GetMemoryStatus(int score)
    {
        if (score >= 14 && score <= 19)
            return "ท่านมีความจำดีเยี่ยม";
        if (score >= 20 && score <= 29)
            return "ความจำดีปานกลาง";
        if (score >= 30 && score <= 39)
            return "ความจำของท่านไม่ดีเท่าไหร่";
        if (score >= 40 && score <= 56)
            return "ควรไปปรึกษาแพทย์";
        return "ไม่ทราบเกณฑ์";
    }
}