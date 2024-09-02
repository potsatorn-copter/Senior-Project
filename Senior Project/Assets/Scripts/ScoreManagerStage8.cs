using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // อย่าลืม import สำหรับ TextMeshPro

public class ScoreManagerStage8 : MonoBehaviour
{
    public static ScoreManagerStage8 Instance;
    public Text  scoreTextGet; // อ้างอิงไปยัง TextMeshPro text สำหรับแสดงคะแนน
    public Text scoreTextLoss;
     int score = 0; // ตัวแปรสำหรับเก็บคะแนนปัจจุบัน
     int lossScore = 0;
    [SerializeField] private GameObject gameOverUI; // GameObject สำหรับแสดง UI เมื่อเกมจบ
    [SerializeField] private GameObject gameWin;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        UpdateScoreText(); // อัพเดท UI เมื่อเริ่มเกม
        gameOverUI.SetActive(false); // ซ่อน UI เมื่อเริ่มเกม
        gameWin.SetActive(false);
    }

    public void AddScore(int pointsToAdd)
    {
        score += pointsToAdd;
        UpdateScoreText(); // อัพเดท UI ทุกครั้งที่คะแนนเปลี่ยนแปลง
    }

    public void SubtractScore(int pointsToSubtract)
    {
        lossScore += pointsToSubtract;
        UpdateScoreText(); // อัพเดท UI ทุกครั้งที่คะแนนเปลี่ยนแปลง
    }

    private void UpdateScoreText()
    {
        if (scoreTextGet != null) // ตรวจสอบว่าอ้างอิงไปยัง TextMeshPro ถูกต้อง
        {
            scoreTextGet.text = "ได้แต้ม  : " + score; // อัพเดทข้อความ UI
        }
        if(scoreTextLoss != null)
        {
            scoreTextLoss.text = "เสียแต้ม : " + lossScore; //
        }
    }
    /*public int GetCurrentScore()
    {
        return score;
    }
    public int GetCurrentLossScore()
    {
        return lossScore;
    }*/
    

    private void GameOver()
    {
        if(lossScore >= 3)
        {
            SoundManager.instance.Play(SoundManager.SoundName.WinSound);
            gameOverUI.SetActive(true); // แสดง UI เมื่อเกมจบ
        }
    }
    private void GameWin()
    {
        if(score >= 3)
        {
            SoundManager.instance.Play(SoundManager.SoundName.WinSound);
            gameWin.SetActive(true); // แสดง UI เมื่อเกมจบ
        }
    }

    private void Update()
    {
        if(lossScore == 3)
        {
            GameOver();
        }
        if(score == 3)
        {
            GameWin();
        }

    }
}
