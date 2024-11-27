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
    private bool isGameEnded = false;

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

    public int GetCurrentScore()
    {
        return score;
    }

    public void EndGameWithJigsawCheck()
    {
        if (isGameEnded) return;
        
        SoundManager.instance.Play(SoundManager.SoundName.WinSound);

        isGameEnded = true;
        CalculateFinalScore();

        if (hasDroppedJigsaw)
        {
            StartCoroutine(ShowJigsawAndEndGamePanelCoroutine());
        }
        else
        {
            ShowEndGamePanel();
        }
    }

    public void CalculateFinalScore()
    {
        int difficultyLevel = GameSettings.difficultyLevel;

        if (difficultyLevel == 0)
        {
            finalScore = score >= 2200 ? 10 : score >= 2000 ? 8 : score >= 1800 ? 6 : score >= 1400 ? 4 : score >= 1000 ? 2 : 0;
        }
        else if (difficultyLevel == 1)
        {
            finalScore = score >= 2300 ? 10 : score >= 2100 ? 8 : score >= 1900 ? 6 : score >= 1500 ? 4 : score >= 1000 ? 2 : 0;
        }
        else if (difficultyLevel == 2)
        {
            finalScore = score >= 2400 ? 10 : score >= 2200 ? 8 : score >= 1800 ? 6 : score >= 1500 ? 4 : score >= 1300 ? 2 : 0;
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = "คะแนนที่ได้ : " + finalScore;
            Debug.Log("Final score set to: " + finalScore);
        }

        ScoreManager.Instance.SetScoreForScene(2, finalScore);
        DropJigsawPieceIfEligible();
    }

    private void DropJigsawPieceIfEligible()
    {
        if (finalScore >= 8)
        {
            int difficultyLevel = GameSettings.difficultyLevel;
            JigsawManager.JigsawImage jigsawImage = JigsawManager.Instance.jigsawImages[difficultyLevel];
            JigsawManager.JigsawPiece droppedPiece = jigsawImage.jigsawPieces[1]; // ตรวจสอบชิ้นส่วนที่ index 1

            // ตรวจสอบว่าชิ้นส่วนนั้นถูกเก็บไปแล้วหรือยัง
            if (!droppedPiece.isCollected)
            {
                // เก็บชิ้นส่วนถ้ายังไม่ถูกเก็บ
                JigsawManager.Instance.CollectJigsawPiece(difficultyLevel, 1);

                if (jigsawUIPanel != null)
                {
                    jigsawUIPanel.ShowJigsawUIPanel(droppedPiece.pieceSprite, "คุณได้รับชิ้นส่วนจิ๊กซอว์ใหม่!");
                    hasDroppedJigsaw = true;
                    Debug.Log("JigsawUIPanel shown with new piece.");
                }
                else
                {
                    Debug.LogWarning("JigsawUIPanel is not found in the scene.");
                    hasDroppedJigsaw = false;
                }
            }
            else
            {
                // แสดงว่าไม่แสดง UI เนื่องจากชิ้นส่วนถูกเก็บไปแล้ว
                hasDroppedJigsaw = false;
                Debug.Log("Jigsaw piece already collected - no UI shown.");
            }
        }
        else
        {
            hasDroppedJigsaw = false;
            Debug.Log("No jigsaw piece dropped - final score below threshold.");
        }
    }

    private IEnumerator ShowJigsawAndEndGamePanelCoroutine()
    {
        if (jigsawUIPanel != null && hasDroppedJigsaw)
        {
            yield return new WaitForSecondsRealtime(3f);
            
            jigsawUIPanel.HideJigsawUIPanel();
        }
        
        yield return new WaitForSecondsRealtime(0.5f);

        ShowEndGamePanel();
    }

    public void ShowEndGamePanel()
    {
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}