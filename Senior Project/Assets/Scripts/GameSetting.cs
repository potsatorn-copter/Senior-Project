using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
    public static int difficultyLevel = 0; // ค่าเริ่มต้นเป็น 0 (ง่าย)

    // ฟังก์ชันนี้เรียกเมื่อเกมเริ่มต้น เพื่อดึงค่าที่บันทึกไว้
    public static void LoadDifficulty()
    {
        // ดึงค่าระดับความยากจาก PlayerPrefs หากไม่มีจะใช้ค่าเริ่มต้นเป็น 0
        difficultyLevel = PlayerPrefs.GetInt("DifficultyLevel", 0);
    }
}