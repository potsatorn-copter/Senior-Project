using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DiffTest : MonoBehaviour
{
    // ปุ่มทั้ง 3 สำหรับการทดสอบระดับ
    public Button easyButton;
    public Button normalButton;
    public Button hardButton;

    void Start()
    {
        // ผูกฟังก์ชันกับการกดปุ่ม
        easyButton.onClick.AddListener(() => SetDifficultyAndReload(0));
        normalButton.onClick.AddListener(() => SetDifficultyAndReload(1));
        hardButton.onClick.AddListener(() => SetDifficultyAndReload(2));
    }

    // ฟังก์ชันสำหรับการตั้งค่าระดับและรีโหลดซีนปัจจุบัน
    private void SetDifficultyAndReload(int difficultyLevel)
    {
        GameSettings.difficultyLevel = difficultyLevel; // ตั้งค่าระดับความยากใน GameSettings
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // รีโหลดซีนปัจจุบัน
    }
}
