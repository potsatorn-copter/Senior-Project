using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // เพิ่มการใช้งาน TextMeshPro

public class ScoreManagerStage4 : MonoBehaviour
{
    public static ScoreManagerStage4 Instance;
    public TextMeshProUGUI scoreTextGet;
    public TextMeshProUGUI newScoreText;
    public TextMeshProUGUI bottlesRemainingText;
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
            scoreTextGet.text = "Valid  : " + score;
        }
        if (newScoreText != null)
        {
            newScoreText.text = "Final Score: " + finalScore;
        }
        if (bottlesRemainingText != null)
        {
            bottlesRemainingText.text = "Bottles Remaining: " + remainingBottles;
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
            else if (score >= 3 && score <= 4)
            {
                finalScore = 4;
            }
            else if (score >= 1 && score <= 2)
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
        gameWinUI.SetActive(true);
        newScoreText.text = "Final Score: " + finalScore;
        Time.timeScale = 0f;
        
        ScoreManager.Instance.SetScoreForScene(4, finalScore);
        Debug.Log("Score for Scene 4 set in ScoreManager: " + finalScore);

        DropJigsawPieceIfEligible(finalScore);
    }

    private void DropJigsawPieceIfEligible(int finalScore)
    {
        if (finalScore >= 8)
        {
            int imageIndex = 0;

            if (GameSettings.difficultyLevel == 0)
            {
                imageIndex = 0;
            }
            else if (GameSettings.difficultyLevel == 1)
            {
                imageIndex = 1;
            }
            else if (GameSettings.difficultyLevel == 2)
            {
                imageIndex = 2;
            }

            JigsawManager.Instance.CollectJigsawPiece(imageIndex, 3);
            Debug.Log($"Jigsaw piece dropped: Scene 4, Difficulty Level: {GameSettings.difficultyLevel}, Image Part 4: {imageIndex}, Final Score: {finalScore}");
        }
        else
        {
            Debug.Log("No jigsaw piece dropped in Scene 4. Final score below threshold.");
        }
    }

    private void Update()
    {
        if (isGameOver) return;
    }
}