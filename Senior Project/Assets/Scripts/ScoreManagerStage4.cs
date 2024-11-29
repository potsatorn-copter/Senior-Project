using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManagerStage4 : MonoBehaviour
{
    public static ScoreManagerStage4 Instance;
    public TextMeshProUGUI scoreTextGet;
    public TextMeshProUGUI newScoreText;
    public TextMeshProUGUI bottlesRemainingText;
    public JigsawUIPanel jigsawUIPanel;
    private int score = 0;
    private int lossScore = 0;
    private int finalScore = 0;
    private int totalThrows = 0;
    private const int maxThrows = 10;
    private int remainingBottles = maxThrows;
    private const int maxMisses = 2;
    [SerializeField] private GameObject gameWinUI;
    private bool isGameOver = false;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        UpdateScoreText();
        gameWinUI.SetActive(false);
    }

    public void AddScore(int pointsToAdd = 1)
    {
        if (!isGameOver)
        {
            totalThrows++;
            remainingBottles--;
            score += pointsToAdd;
            UpdateScoreText();
            CheckGameStatus();
        }
    }

    public void SubtractScore(int pointsToSubtract = 1)
    {
        if (!isGameOver)
        {
            totalThrows++;
            remainingBottles--;
            lossScore++;
            if (lossScore > maxMisses)
            {
                score -= pointsToSubtract;
                score = Mathf.Max(0, score);
            }
            UpdateScoreText();
            CheckGameStatus();
        }
    }

    private void UpdateScoreText()
    {
        if (scoreTextGet != null)
        {
            scoreTextGet.text = "ปาลง: " + score;
        }
        if (newScoreText != null)
        {
            newScoreText.text = "Final Score: " + finalScore;
        }
        if (bottlesRemainingText != null)
        {
            bottlesRemainingText.text = "เหลือขวด: " + remainingBottles;
        }
    }

    private void CheckGameStatus()
    {
        if (score >= 6 || totalThrows >= maxThrows)
        {
            if (score >= 6)
            {
                finalScore = 10;
            }
            else if (score == 5)
            {
                finalScore = 8;
            }
            else if (score == 4)
            {
                finalScore = 6;
            }
            else if (score == 3)
            {
                finalScore = 4;
            }
            else if (score == 1 || score == 2)
            {
                finalScore = 2;
            }
            else
            {
                finalScore = 0;
            }

            TriggerGameWin();
        }
    }

    private void TriggerGameWin()
    {
        isGameOver = true;
        SoundManager.instance.Play(SoundManager.SoundName.WinSound);

        newScoreText.text = "คะแนนที่ได้: " + finalScore;
        ScoreManager.Instance.SetScoreForScene(4, finalScore);
        Debug.Log("Score for Scene 4 set in ScoreManager: " + finalScore);

        DropJigsawPieceIfEligible(finalScore);
    }

    private void DropJigsawPieceIfEligible(int finalScore)
    {
        if (finalScore >= 8)
        {
            int imageIndex = GameSettings.difficultyLevel;
            JigsawManager.JigsawImage jigsawImage = JigsawManager.Instance.jigsawImages[imageIndex];
            JigsawManager.JigsawPiece droppedPiece = jigsawImage.jigsawPieces[3]; // ชิ้นส่วนที่ index 3

            // ตรวจสอบว่าชิ้นส่วนนั้นถูกเก็บไปแล้วหรือไม่
            if (!droppedPiece.isCollected)
            {
                // เก็บชิ้นส่วนถ้ายังไม่ถูกเก็บ
                JigsawManager.Instance.CollectJigsawPiece(imageIndex, 3);

                if (jigsawUIPanel != null)
                {
                    Sprite jigsawSprite = droppedPiece.pieceSprite;
                    jigsawUIPanel.ShowJigsawUIPanel(jigsawSprite, "คุณได้รับชิ้นส่วนจิ๊กซอว์ใหม่!");
                    StartCoroutine(ShowEndGamePanelWithDelay(jigsawUIPanel));
                }
                else
                {
                    ShowEndGamePanel();
                }
            }
            else
            {
                // แสดงข้อความแจ้งเตือนว่าชิ้นส่วนนี้ถูกเก็บไปแล้ว
                Debug.Log("Jigsaw piece already collected - no UI shown.");
                ShowEndGamePanel();
            }
        }
        else
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
        if (gameWinUI != null)
        {
            gameWinUI.SetActive(true);
            Time.timeScale = 0f; // หยุดเวลาเมื่อแสดง Game Win UI
        }
    }
}