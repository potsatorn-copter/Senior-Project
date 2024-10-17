using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // เพิ่มการใช้งาน TextMeshPro

public class ScoreManagerStage4 : MonoBehaviour
{
    public static ScoreManagerStage4 Instance;
    public TextMeshProUGUI scoreTextGet;
    public TextMeshProUGUI newScoreText;  // TextMeshProUGUI สำหรับแสดงคะแนนใหม่
    public TextMeshProUGUI bottlesRemainingText;  // TextMeshProUGUI สำหรับแสดงจำนวนขวดที่เหลือ
    private int score = 0;
    private int lossScore = 0;
    private int newScore = 0;  // ตัวแปรเก็บคะแนนชนิดใหม่
    private int totalThrows = 0; // จำนวนการปาทั้งหมด
    private const int maxThrows = 10; // จำนวนการปาสูงสุดคือ 10 ครั้ง
    private int remainingBottles = maxThrows; // จำนวนขวดที่เหลือจะเริ่มต้นที่ 10
    private const int maxMisses = 2; // จำนวนพลาดสูงสุดที่อนุญาตก่อนจะเริ่มหักคะแนน
    [SerializeField] private GameObject gameWinUI;
    private bool isGameOver = false; // ตัวแปรบอกสถานะเกม

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        UpdateScoreText();
        gameWinUI.SetActive(false);
    }

    // เรียกฟังก์ชันนี้เมื่อขวดตกลงถังเท่านั้น
    public void AddScore(int pointsToAdd = 1)
    {
        if (!isGameOver)
        {
            totalThrows++; // เพิ่มจำนวนการปาทุกครั้งที่ได้คะแนน
            remainingBottles--; // ลดจำนวนขวดที่เหลือลงทุกครั้งที่ปา
            score += pointsToAdd; // เพิ่มคะแนนเฉพาะเมื่อลงถัง
            UpdateScoreText();
            CheckGameStatus(); // ตรวจสอบสถานะเกมหลังจากเพิ่มคะแนน
        }
    }

    // เรียกฟังก์ชันนี้เมื่อผู้เล่นปาพลาด
    public void SubtractScore(int pointsToSubtract = 1)
    {
        if (!isGameOver)
        {
            totalThrows++; // เพิ่มจำนวนการปาทุกครั้งที่พลาด
            remainingBottles--; // ลดจำนวนขวดที่เหลือลงทุกครั้งที่ปา แม้ว่าจะพลาด
            lossScore++; // เพิ่มจำนวนครั้งที่พลาด
            if (lossScore > maxMisses)
            {
                score -= pointsToSubtract; // เริ่มหักคะแนนเมื่อลงขวดไม่ได้เกิน 2 ครั้ง
                score = Mathf.Max(0, score); // ป้องกันไม่ให้คะแนนติดลบ
            }
            UpdateScoreText();
            CheckGameStatus(); // ตรวจสอบสถานะเกมหลังจากลบคะแนน
        }
    }

    private void UpdateScoreText()
    {
        if (scoreTextGet != null)
        {
            scoreTextGet.text = "Valid  : " + score; // แสดงจำนวนขวดที่ปาลงจริง
        }
        if (newScoreText != null)
        {
            newScoreText.text = "New Score: " + newScore; // แสดงคะแนนใหม่ที่คำนวณได้
        }
        if (bottlesRemainingText != null)
        {
            bottlesRemainingText.text = "Bottles Remaining: " + remainingBottles; // แสดงจำนวนขวดที่เหลือ
        }
    }

    private void CheckGameStatus()
    {
        // ถ้าผู้เล่นปาขวดลงครบ 6 ครั้ง หรือปาครบ 10 ครั้ง เกมจะจบ
        if (score >= 6 || totalThrows >= maxThrows)
        {
            // คำนวณคะแนนชนิดใหม่ตามจำนวนการปาสำเร็จ
            if (score >= 6)
            {
                newScore = 10; // ปาได้ 6 ขวดขึ้นไป
            }
            else if (score == 5)
            {
                newScore = 8; // ปาได้ 5 ขวด
            }
            else if (score >= 3 && score <= 4)
            {
                newScore = 4; // ปาได้ 3-4 ขวด
            }
            else if (score >= 1 && score <= 2)
            {
                newScore = 2; // ปาได้ 1-2 ขวด
            }
            else
            {
                newScore = 0; // ไม่ได้ขวดเลย
            }

            TriggerGameWin();
        }
    }

    private void TriggerGameWin()
    {
        isGameOver = true; // ตั้งค่าเป็นเกมจบ
        SoundManager.instance.Play(SoundManager.SoundName.WinSound); // แสดงผลเมื่อเกมจบ
        gameWinUI.SetActive(true);
        newScoreText.text = "Final Score: " + newScore; // แสดงคะแนนใหม่สุดท้าย
        Time.timeScale = 0f; // หยุดการทำงานของเกม
        
        ScoreManager.Instance.SetScoreForScene(4, newScore); // ซีนที่ 4
        Debug.Log("Score for Scene 4 set in ScoreManager: " + newScore);
    }

    private void Update()
    {
        if (isGameOver) return; // ถ้าเกมจบแล้ว ไม่ต้องทำอะไรต่อ
    }
}