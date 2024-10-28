using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ScoremanagerScene2 : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
    public JigsawUIPanel jigsawUIPanel;
    public GameObject endGamePanel;
    
    private int score = 0;
    private int finalScore = 0;
    private bool hasDroppedJigsaw = false;

    private void Start()
    {
        UpdateScoreText();

        if (endGamePanel != null)
        {
            endGamePanel.SetActive(false);
        }
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null) scoreText.text = "คะแนน: " + score;
    }

    public void CheckEndGameCondition()
    {
        if (score >= 100) // เงื่อนไขคะแนนที่ต้องการเพื่อสิ้นสุดเกม
        {
            CalculateFinalScore();

            if (hasDroppedJigsaw)
            {
                StartCoroutine(ShowEndGamePanelWithDelay());
            }
            else
            {
                ShowEndGamePanel();
            }
        }
    }

    public void CalculateFinalScore()
    {
        int difficultyLevel = GameSettings.difficultyLevel;

        if (difficultyLevel == 0) // ง่าย
        {
            if (score >= 2200)
                finalScore = 10;
            else if (score >= 2000)
                finalScore = 8;
            else if (score >= 1800)
                finalScore = 6;
            else if (score >= 1400)
                finalScore = 4;
            else if (score >= 1000)
                finalScore = 2;
            else
                finalScore = 0;
        }
        else if (difficultyLevel == 1) // กลาง
        {
            if (score >= 2300)
                finalScore = 10;
            else if (score >= 2000)
                finalScore = 8;
            else if (score >= 1900)
                finalScore = 6;
            else if (score >= 1500)
                finalScore = 4;
            else if (score >= 1000)
                finalScore = 2;
            else
                finalScore = 0;
        }
        else if (difficultyLevel == 2) // ยาก
        {
            if (score >= 2200)
                finalScore = 10;
            else if (score >= 1800)
                finalScore = 8;
            else if (score >= 1700)
                finalScore = 6;
            else if (score >= 1500)
                finalScore = 4;
            else if (score >= 1300)
                finalScore = 2;
            else
                finalScore = 0;
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = "คะแนนที่ได้ : " + finalScore;
        }
        Debug.Log("Setting score for Scene 2 in ScoreManager: " + finalScore);
        ScoreManager.Instance.SetScoreForScene(2, finalScore);
        DropJigsawPieceIfEligible();
    }

    private void DropJigsawPieceIfEligible()
    {
        if (finalScore >= 8)
        {
            int difficultyLevel = GameSettings.difficultyLevel;

            JigsawManager.Instance.CollectJigsawPiece(difficultyLevel, 1);
            JigsawManager.JigsawImage jigsawImage = JigsawManager.Instance.jigsawImages[difficultyLevel];
            JigsawManager.JigsawPiece droppedPiece = jigsawImage.jigsawPieces[1]; // ดึงชิ้นส่วนจิ๊กซอว์ที่เหมาะสม

            if (jigsawUIPanel != null)
            {
                jigsawUIPanel.ShowJigsawUIPanel(droppedPiece.pieceSprite, "You have collected a jigsaw piece!"); // ส่ง Sprite ไปแสดง
                hasDroppedJigsaw = true;
            }
            else
            {
                Debug.LogWarning("JigsawUIPanel is not assigned.");
                hasDroppedJigsaw = false;
            }
        }
        else
        {
            hasDroppedJigsaw = false;
            Debug.Log("No jigsaw piece dropped. Final score below threshold.");
        }
    }

    private IEnumerator ShowEndGamePanelWithDelay()
    {
        yield return new WaitForSeconds(3f);

        if (jigsawUIPanel != null)
        {
            jigsawUIPanel.HideJigsawUIPanel();
        }

        yield return new WaitForSeconds(0.5f);
        ShowEndGamePanel();
    }

    private void ShowEndGamePanel()
    {
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    public void EndGame()
    {
        CalculateFinalScore();
        Debug.Log("Final score calculated: " + finalScore);
    }
}