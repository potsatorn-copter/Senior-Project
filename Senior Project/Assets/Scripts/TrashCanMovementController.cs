using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrashCanMovementController : MonoBehaviour
{
    [SerializeField] private MovingPlatform movingPlatform; // อ้างอิงถึง MovingPlatform
    [SerializeField]  private Obstaclebin Obstaclebin;
    [SerializeField]  private GameObject Monster;
    [SerializeField] private Button easyButton;  // ปุ่มสำหรับ Easy
    [SerializeField] private Button normalButton;  // ปุ่มสำหรับ Normal
    [SerializeField] private Button hardButton;  // ปุ่มสำหรับ Hard

    void Start()
    {
        // เชื่อมปุ่มกับฟังก์ชัน
        easyButton.onClick.AddListener(SetEasyMode);
        normalButton.onClick.AddListener(SetNormalMode);
        hardButton.onClick.AddListener(SetHardMode);

        // ตรวจสอบระดับความยาก
        AdjustDifficulty(GameSettings.difficultyLevel);
    }

    // ฟังก์ชันสำหรับปรับการเคลื่อนไหวถังขยะตามระดับความยาก
    private void AdjustDifficulty(int difficultyLevel)
    {
        if (difficultyLevel == 0) // Easy
        {
            movingPlatform.enabled = true; // เปิดการขยับถังขยะ
            movingPlatform.speed = 1f; // ตั้งความเร็วให้ช้ากว่าโหมดอื่น
            Monster.SetActive(false);
        }
        else if (difficultyLevel == 1) // Normal
        {
            movingPlatform.enabled = true; // เปิดการขยับถังขยะ
            movingPlatform.speed = 2f; // ตั้งค่าความเร็วปกติสำหรับ Normal
            Monster.SetActive(false);
        }
        else if (difficultyLevel == 2) // Hard
        {
            movingPlatform.enabled = true; // เปิดการขยับถังขยะ
            movingPlatform.speed = 2f; // ตั้งค่าความเร็วปกติสำหรับ Hard
            Obstaclebin.enabled = true;
            Monster.SetActive(true);
        }
    }

    // ฟังก์ชันที่ถูกเรียกเมื่อกดปุ่ม Easy
    public void SetEasyMode()
    {
        GameSettings.difficultyLevel = 0;  // ตั้งค่า Easy
        ReloadScene(); // รีโหลด Scene เพื่อให้การเปลี่ยนระดับมีผล
    }

    // ฟังก์ชันที่ถูกเรียกเมื่อกดปุ่ม Normal
    public void SetNormalMode()
    {
        GameSettings.difficultyLevel = 1;  // ตั้งค่า Normal
        ReloadScene(); // รีโหลด Scene เพื่อให้การเปลี่ยนระดับมีผล
    }

    // ฟังก์ชันที่ถูกเรียกเมื่อกดปุ่ม Hard
    public void SetHardMode()
    {
        GameSettings.difficultyLevel = 2;  // ตั้งค่า Hard
        ReloadScene(); // รีโหลด Scene เพื่อให้การเปลี่ยนระดับมีผล
    }

    // ฟังก์ชันสำหรับรีโหลด Scene ปัจจุบัน
    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}