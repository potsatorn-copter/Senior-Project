using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalScoreDisplay : MonoBehaviour
{
    // TextMeshPro สำหรับแสดงคะแนนแต่ละซีน
    public TextMeshProUGUI scoreScene1Text;
    public TextMeshProUGUI scoreScene2Text;
    public TextMeshProUGUI scoreScene3Text;
    public TextMeshProUGUI scoreScene4Text;
    public TextMeshProUGUI scoreScene5Text;

    public float typingSpeed = 0.05f;
    public float fadeDuration = 0.5f; 

    // TextMeshPro สำหรับแสดงคะแนนรวม
    public TextMeshProUGUI totalScoreText;

    void Start()
    {
        // ดึงคะแนนจาก ScoreManager
        int scoreScene1 = ScoreManager.Instance.scoreScene1;
        int scoreScene2 = ScoreManager.Instance.scoreScene2;
        int scoreScene3 = ScoreManager.Instance.scoreScene3;
        int scoreScene4 = ScoreManager.Instance.scoreScene4;
        int scoreScene5 = ScoreManager.Instance.scoreScene5;

        // คำนวณคะแนนรวม
        int totalScore = ScoreManager.Instance.GetTotalScore();

        if (scoreScene1Text != null) StartCoroutine(TypeText(scoreScene1Text, $"Score Scene 1: {scoreScene1}/10"));
        if (scoreScene2Text != null) StartCoroutine(TypeText(scoreScene2Text, $"Score Scene 2: {scoreScene2}/10"));
        if (scoreScene3Text != null) StartCoroutine(TypeText(scoreScene3Text, $"Score Scene 3: {scoreScene3}/10"));
        if (scoreScene4Text != null) StartCoroutine(TypeText(scoreScene4Text, $"Score Scene 4: {scoreScene4}/10"));
        if (scoreScene5Text != null) StartCoroutine(TypeText(scoreScene5Text, $"Score Scene 5: {scoreScene5}/10"));

        if (scoreScene5Text != null) StartCoroutine(ShowAllScores(totalScoreText, $"Total Score: {totalScore}/50"));

        // อัปเดต TextMeshPro เพื่อแสดงคะแนนแต่ละซีน
        /*if (scoreScene1Text != null) scoreScene1Text.text = "Score Scene 1: " + scoreScene1 + "/10";
        if (scoreScene2Text != null) scoreScene2Text.text = "Score Scene 2: " + scoreScene2 + "/10";
        if (scoreScene3Text != null) scoreScene3Text.text = "Score Scene 3: " + scoreScene3 + "/10";
        if (scoreScene4Text != null) scoreScene4Text.text = "Score Scene 4: " + scoreScene4 + "/10";
        if (scoreScene5Text != null) scoreScene5Text.text = "Score Scene 5: " + scoreScene5 + "/10";

        // อัปเดต TextMeshPro เพื่อแสดงคะแนนรวม
        if (totalScoreText != null) totalScoreText.text = "Total Score: " + totalScore + "/50";*/
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
        textComponent.text = ""; // เคลียร์ข้อความก่อนเริ่ม

        foreach (char letter in message.ToCharArray())
        {
            textComponent.text += letter; // เพิ่มตัวอักษรทีละตัว
            yield return new WaitForSeconds(typingSpeed); // หน่วงเวลา
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
}
