using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class FinalScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreScene1Text;
    public TextMeshProUGUI scoreScene2Text;
    public TextMeshProUGUI scoreScene3Text;
    public TextMeshProUGUI scoreScene4Text;
    public TextMeshProUGUI scoreScene5Text;
    public TextMeshProUGUI totalScoreText;
    public float typingSpeed = 0.05f;
    public float fadeDuration = 0.5f;

    private int scoreScene1, scoreScene2, scoreScene3, scoreScene4, scoreScene5, totalScore;

    async void Awake()
    {
        // ดึงคะแนนจาก ScoreManager
        scoreScene1 = ScoreManager.Instance.scoreScene1;
        scoreScene2 = ScoreManager.Instance.scoreScene2;
        scoreScene3 = ScoreManager.Instance.scoreScene3;
        scoreScene4 = ScoreManager.Instance.scoreScene4;
        scoreScene5 = ScoreManager.Instance.scoreScene5;
        totalScore = ScoreManager.Instance.GetTotalScore();
        

        // เริ่มต้น Unity Services
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
    }

    void Start()
    {
        string criteria = GetCriteria(totalScore);
        // แสดงคะแนนใน UI
        if (scoreScene1Text != null) StartCoroutine(TypeText(scoreScene1Text, $"Score Scene 1: {scoreScene1}/10"));
        if (scoreScene2Text != null) StartCoroutine(TypeText(scoreScene2Text, $"Score Scene 2: {scoreScene2}/10"));
        if (scoreScene3Text != null) StartCoroutine(TypeText(scoreScene3Text, $"Score Scene 3: {scoreScene3}/10"));
        if (scoreScene4Text != null) StartCoroutine(TypeText(scoreScene4Text, $"Score Scene 4: {scoreScene4}/10"));
        if (scoreScene5Text != null) StartCoroutine(TypeText(scoreScene5Text, $"Score Scene 5: {scoreScene5}/10"));
        if (totalScoreText != null) StartCoroutine(ShowAllScores(totalScoreText, $"Total Score: {totalScore}/50\n{criteria}"));

        // ส่ง Analytics ทันที
        SendAnalytics(scoreScene1, scoreScene2, scoreScene3, scoreScene4, scoreScene5, totalScore);
    }

    private void SendAnalytics(int scoreScene1, int scoreScene2, int scoreScene3, int scoreScene4, int scoreScene5, int totalScore)
    {
        // สร้าง Event FinalScore
        var finalScoreEvent = new CustomEvent("FinalScore")
        {
            { "ScoreScene1", $"มินิเกม 1 ได้คะแนน {scoreScene1}/10" },
            { "ScoreScene2", $"มินิเกม 2 ได้คะแนน {scoreScene2}/10" },
            { "ScoreScene3", $"มินิเกม 3 ได้คะแนน {scoreScene3}/10" },
            { "ScoreScene4", $"มินิเกม 4 ได้คะแนน {scoreScene4}/10" },
            { "ScoreScene5", $"มินิเกม 5 ได้คะแนน {scoreScene5}/10" },
            { "Difficulty", GetDifficultyString(GameSettings.difficultyLevel) }
        };

        AnalyticsService.Instance.RecordEvent(finalScoreEvent);

        // สร้าง Event ScoreAmount
        var scoreAmountEvent = new CustomEvent("ScoreAmount")
        {
            { "ScoreAmountString", $"คะแนนรวมของคุณคือ {totalScore}/50" },
            { "Difficulty", GetDifficultyString(GameSettings.difficultyLevel) }
        };

        AnalyticsService.Instance.RecordEvent(scoreAmountEvent);

        Debug.Log("Analytics events sent using CustomEvent.");
    }

    private string GetDifficultyString(int difficultyLevel)
    {
        switch (difficultyLevel)
        {
            case 0: return "ง่าย";
            case 1: return "กลาง";
            case 2: return "ยาก";
            default: return "ไม่ทราบ";
        }
    }

    private IEnumerator ShowAllScores(params object[] textsAndMessages)
    {
        for (int i = 0; i < textsAndMessages.Length; i += 2)
        {
            var textComponent = (TextMeshProUGUI)textsAndMessages[i];
            var message = (string)textsAndMessages[i + 1];
            if (textComponent != null)
            {
                StartCoroutine(TypeAndFadeText(textComponent, message));
                yield return new WaitForSeconds(typingSpeed * message.Length);
            }
        }
    }

    private IEnumerator TypeText(TextMeshProUGUI textComponent, string message)
    {
        textComponent.text = "";
        foreach (char letter in message.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private IEnumerator TypeAndFadeText(TextMeshProUGUI textComponent, string message)
    {
        textComponent.text = message;
        Color originalColor = textComponent.color;
        originalColor.a = 0;
        textComponent.color = originalColor;

        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
    }
   
    private string GetCriteria(int totalScore)
    {
        if (totalScore >= 46 && totalScore <= 50)
            return "สุดยอด";
        else if (totalScore >= 40 && totalScore <= 45)
            return "เก่งมาก";
        else if (totalScore >= 30 && totalScore <= 35)
            return "เก่ง";
        else if (totalScore >= 20 && totalScore <= 25)
            return "ทั่วไป";
        else if (totalScore >= 10 && totalScore <= 15)
            return "พยายามอีกนิดนะ";
        else
            return "สู้ๆพยายามเข้า";
    }
}