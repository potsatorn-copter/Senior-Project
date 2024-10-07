using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyManager : MonoBehaviour
{
    public void SetDifficulty(int difficultyLevel)
    {
        // เก็บระดับความยากที่เลือกใน GameSettings
        GameSettings.difficultyLevel = difficultyLevel;

        // เปลี่ยน Scene ไปยังหน้าถัดไปที่เลือกมินิเกม
        SceneManager.LoadScene("Stage"); // เปลี่ยนเป็นชื่อ Scene ที่ใช้
    }
}
