using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AImanager : MonoBehaviour
{
    public SmartAI aiHand1; // มือ AI ตัวแรก
    public SmartAI aiHand2; // มือ AI ตัวที่สอง (เก่งกว่า)

    private void Start()
    {
        // ตรวจสอบระดับความยากจาก GameSettings
        int difficulty = GameSettings.difficultyLevel;

        // เปิดมือ AI ตัวแรกเสมอ
        aiHand1.gameObject.SetActive(true);

        // เปิดใช้มือ AI ตัวที่สองเฉพาะในระดับยาก (Hard)
        if (difficulty == 2)
        {
            aiHand2.gameObject.SetActive(true);  // เปิดใช้มือ AI ตัวที่สอง
        }
        else
        {
            aiHand2.gameObject.SetActive(false); // ปิดมือ AI ตัวที่สองสำหรับ Easy และ Normal
        }
    }
}
