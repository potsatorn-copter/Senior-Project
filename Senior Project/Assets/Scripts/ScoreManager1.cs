using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager1 : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
    public GameObject endGamePanel;
    private int goodItemCount = 0;
    private int finalScore = 0;

    private Dictionary<int, int[]> difficultyThresholds = new Dictionary<int, int[]>()
    {
        { 0, new int[] { 11, 9, 6, 3, 1 } },
        { 1, new int[] { 14, 12, 9, 5, 1 } },
        { 2, new int[] { 15, 13, 9, 5, 1 } }
    };

    private void Start()
    {
        UpdateScoreText();
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(false);
        }
    }

    public void CollectGoodItem()
    {
        goodItemCount++;
        UpdateScoreText();
    }

    public void CollectBadItem()
    {
        goodItemCount--;
        UpdateScoreText();
    }

    public void CalculateFinalScore()
    {
        int difficulty = GameSettings.difficultyLevel;
        int[] thresholds = difficultyThresholds[difficulty];

        if (goodItemCount >= thresholds[0])
            finalScore = 10;
        else if (goodItemCount >= thresholds[1])
            finalScore = 8;
        else if (goodItemCount >= thresholds[2])
            finalScore = 6;
        else if (goodItemCount >= thresholds[3])
            finalScore = 4;
        else if (goodItemCount >= thresholds[4])
            finalScore = 2;
        else
            finalScore = 0;

        if (finalScoreText != null)
        {
            finalScoreText.text = "คะแนนที่ได้ : " + finalScore;
        }

        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(false);
        }

        Debug.Log("Setting score for Scene 1 in ScoreManager: " + finalScore);
        ScoreManager.Instance.SetScoreForScene(1, finalScore);

        DropJigsawPieceIfEligible();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "คะแนน: " + goodItemCount;
        }
    }

    private void DropJigsawPieceIfEligible()
    {
        bool jigsawDropped = false;

        if (finalScore >= 8)
        {
            int difficultyLevel = GameSettings.difficultyLevel;
            JigsawManager.Instance.CollectJigsawPiece(difficultyLevel, 0);

            JigsawManager.JigsawImage jigsawImage = JigsawManager.Instance.jigsawImages[difficultyLevel];
            JigsawManager.JigsawPiece droppedPiece = jigsawImage.jigsawPieces[0];

            // แสดง JigsawUIPanel ทุกครั้ง ไม่สนใจว่าเก็บแล้วหรือยัง
            jigsawDropped = true;

            JigsawUIPanel jigsawUIPanel = FindObjectOfType<JigsawUIPanel>();
            if (jigsawUIPanel != null)
            {
                jigsawUIPanel.ShowJigsawUIPanel(droppedPiece.pieceSprite, "You have collected a jigsaw piece!");
                StartCoroutine(ShowEndGamePanelWithDelay(jigsawUIPanel));
            }
            else
            {
                Debug.LogWarning("JigsawUIPanel is not found in the scene.");
                ShowEndGamePanel();
            }

            Debug.Log($"Jigsaw piece dropped: Scene 1, Difficulty Level: {difficultyLevel}, Image Part: 1, Final Score: {finalScore}");
        }

        if (!jigsawDropped)
        {
            ShowEndGamePanel();
        }
    }

    private IEnumerator ShowEndGamePanelWithDelay(JigsawUIPanel jigsawUIPanel)
    {
        yield return new WaitForSeconds(3f);
        jigsawUIPanel.HideJigsawUIPanel();
        yield return new WaitForSeconds(0.5f);
        ShowEndGamePanel();
    }

    private void ShowEndGamePanel()
    {
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(true);
        }
    }
}