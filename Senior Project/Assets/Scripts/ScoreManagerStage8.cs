using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManagerStage8 : MonoBehaviour
{
    public static ScoreManagerStage8 Instance;
    public Text scoreTextGet;
    public Text scoreTextLoss;
    private int score = 0;
    private int lossScore = 0;
    private int totalThrows = 0; // จำนวนการปาทั้งหมด
    private const int maxThrows = 6; // จำนวนการปาที่ต้องทำให้ได้
    private const int maxMisses = 2; // จำนวนพลาดสูงสุดที่อนุญาต
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameWinUI;

    private bool isGameOver = false; // ตัวแปรบอกสถานะเกม

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        UpdateScoreText();
        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);
    }

    public void AddScore(int pointsToAdd)
    {
        if (!isGameOver)
        {
            totalThrows++; // เพิ่มจำนวนการปาทุกครั้งที่ได้คะแนน
            score += pointsToAdd;
            UpdateScoreText();
            CheckGameStatus(); // ตรวจสอบสถานะเกมหลังจากเพิ่มคะแนน
        }
    }

    public void SubtractScore(int pointsToSubtract)
    {
        if (!isGameOver)
        {
            lossScore += pointsToSubtract;
            UpdateScoreText();
            CheckGameStatus(); // ตรวจสอบสถานะเกมหลังจากลบคะแนน
        }
    }

    private void UpdateScoreText()
    {
        if (scoreTextGet != null)
        {
            scoreTextGet.text = "ได้แต้ม  : " + score;
        }
        if (scoreTextLoss != null)
        {
            scoreTextLoss.text = "เสียแต้ม : " + lossScore;
        }
    }

    private void CheckGameStatus()
    {
        // เช็คสถานะการแพ้หรือชนะ
        if (lossScore > maxMisses) // ถ้าพลาดเกินจำนวนที่อนุญาต
        {
            TriggerGameOver();
        }
        else if (totalThrows >= maxThrows) // ถ้าปาครบจำนวนที่กำหนด
        {
            TriggerGameWin();
        }
    }

    private void TriggerGameOver()
    {
        isGameOver = true; // ตั้งค่าเป็นเกมจบ
        SoundManager.instance.Play(SoundManager.SoundName.WinSound);
        gameOverUI.SetActive(true);
        Time.timeScale = 0f; // หยุดการทำงานของเกม
    }

    private void TriggerGameWin()
    {
        isGameOver = true; // ตั้งค่าเป็นเกมชนะ
        SoundManager.instance.Play(SoundManager.SoundName.WinSound);
        gameWinUI.SetActive(true);
        Time.timeScale = 0f; // หยุดการทำงานของเกม
    }

    private void Update()
    {
        if (isGameOver) return; // ถ้าเกมจบแล้ว ไม่ต้องทำอะไรต่อ
    }
}
