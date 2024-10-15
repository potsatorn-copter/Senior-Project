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

        // อัปเดต TextMeshPro เพื่อแสดงคะแนนแต่ละซีน
        if (scoreScene1Text != null) scoreScene1Text.text = "Score Scene 1: " + scoreScene1 + "/10";
        if (scoreScene2Text != null) scoreScene2Text.text = "Score Scene 2: " + scoreScene2 + "/10";
        if (scoreScene3Text != null) scoreScene3Text.text = "Score Scene 3: " + scoreScene3 + "/10";
        if (scoreScene4Text != null) scoreScene4Text.text = "Score Scene 4: " + scoreScene4 + "/10";
        if (scoreScene5Text != null) scoreScene5Text.text = "Score Scene 5: " + scoreScene5 + "/10";

        // อัปเดต TextMeshPro เพื่อแสดงคะแนนรวม
        if (totalScoreText != null) totalScoreText.text = "Total Score: " + totalScore + "/50";
    }
}
