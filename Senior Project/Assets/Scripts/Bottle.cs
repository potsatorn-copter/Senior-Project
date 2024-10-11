using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    private static int totalThrows = 0; // ติดตามจำนวนการปา
    private static int totalScore = 0; // คะแนนสะสม
    private const int maxThrows = 6; // จำนวนการปาสูงสุดที่ต้องการ
    private const int maxMisses = 2; // จำนวนพลาดสูงสุดที่อนุญาต
    private int misses = 0; // จำนวนพลาด

    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D other) // แก้ไขให้ถูกต้อง
    {
        totalThrows++; // เพิ่มจำนวนการปาทุกครั้งที่มีการปา

        if (other.gameObject.CompareTag("GoodItem"))
        {
            SoundManager.instance.Play(SoundManager.SoundName.Correct);
            // บวกคะแนนตามจำนวนการปา
            totalScore += CalculateScore(totalThrows);
            ScoreManagerStage8.Instance.AddScore(CalculateScore(totalThrows));
            Destroy(gameObject);
            Debug.Log("+1 Score");
        }
        else if (other.gameObject.CompareTag("BadItem"))
        {
            SoundManager.instance.Play(SoundManager.SoundName.Wrong);
            // ลบคะแนน
            ScoreManagerStage8.Instance.SubtractScore(1);
            misses++; // เพิ่มจำนวนพลาด
            Destroy(gameObject);
            Debug.Log("-1 Score");
        }
    }

    private int CalculateScore(int throws)
    {
        if (throws == 6) return 10;
        if (throws == 5) return 8;
        if (throws >= 3 && throws <= 4) return 4;
        if (throws >= 1 && throws <= 2) return 2;
        return 0;
    }

    void ResetGame()
    {
        totalThrows = 0; // รีเซ็ตจำนวนการปา
        totalScore = 0; // รีเซ็ตคะแนน
        misses = 0; // รีเซ็ตจำนวนพลาด
    }
}
