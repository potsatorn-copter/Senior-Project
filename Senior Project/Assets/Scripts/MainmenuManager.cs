using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainmenuManager : MonoBehaviour
{
    public Button startGameButton;  // ปุ่มสำหรับ Start Game
    public Button continueButton;   // ปุ่มสำหรับ Continue Game
    public Button testButton;       // ปุ่มสำหรับทำแบบทดสอบ

    public HorizontalLayoutGroup layoutGroup; // Horizontal Layout Group ที่ควบคุมปุ่มทั้งหมด
    public float continueActiveSpacing = -44.05f; // ค่า Spacing เมื่อปุ่ม Continue แสดงอยู่
    public float continueInactiveSpacing = 100f;  // ค่า Spacing เมื่อปุ่ม Continue ซ่อนอยู่
    public int continueActiveLeftPadding = -456;  // ค่า Left Padding เมื่อปุ่ม Continue แสดงอยู่
    public int continueInactiveLeftPadding = -521;// ค่า Left Padding เมื่อปุ่ม Continue ซ่อนอยู่

    void Start()
    {
        // โหลดระดับความยากที่เคยถูกบันทึกไว้
        LoadDifficulty();

        // ซ่อนปุ่ม Continue ไว้ก่อน
        continueButton.gameObject.SetActive(false);

        // ตรวจสอบว่ามีการเล่นค้างไว้หรือไม่
        if (PlayerPrefs.HasKey("LastScenePlayed"))
        {
            // ถ้ามีการเล่นค้างไว้ แสดงปุ่ม Continue
            continueButton.gameObject.SetActive(true);

            // Debug ซีนที่ค้างไว้
            int lastScene = PlayerPrefs.GetInt("LastScenePlayed");
            Debug.Log("Scene ที่ค้างไว้: " + lastScene);

            // ผูกปุ่มให้ทำงานเพื่อไปยังซีนที่ค้างไว้
            continueButton.onClick.AddListener(ContinueGame);

            // ปรับค่า Spacing และ Left Padding เมื่อปุ่ม Continue แสดง
            layoutGroup.spacing = continueActiveSpacing;
            layoutGroup.padding.left = continueActiveLeftPadding;
        }
        else
        {
            // ถ้าไม่มีการเล่นค้างไว้ ให้ขยับปุ่ม Start และ Test โดยปรับค่า Spacing และ Left Padding
            layoutGroup.spacing = continueInactiveSpacing;
            layoutGroup.padding.left = continueInactiveLeftPadding;
        }

        // ผูกปุ่มให้เริ่มเกมใหม่ตั้งแต่ซีนแรก
        startGameButton.onClick.AddListener(StartNewGame);
    }

    // ฟังก์ชันสำหรับเริ่มเกมใหม่
    void StartNewGame()
    {
        // ลบข้อมูลเก่าที่ค้างไว้ (ถ้ามี)
        PlayerPrefs.DeleteKey("LastScenePlayed");

        // เริ่มเกมที่ซีนแรก (เช่น ซีน 1)
        SceneManager.LoadScene("Level"); // หรือใส่ชื่อซีนที่คุณต้องการ
    }

    // ฟังก์ชันสำหรับเล่นเกมต่อจากที่ค้างไว้
    void ContinueGame()
    {
        // ดึงซีนล่าสุดที่เล่นจาก PlayerPrefs
        int lastScene = PlayerPrefs.GetInt("LastScenePlayed", SceneManager.GetSceneByName("Level").buildIndex); // ถ้าไม่มีให้ไปซีน "Level"

        // โหลดซีนล่าสุดที่เล่นค้างไว้
        SceneManager.LoadScene(lastScene);
    }

    // ฟังก์ชันสำหรับบันทึกความยากเมื่อเลือก
    public void SaveDifficulty(int difficultyLevel)
    {
        // เก็บระดับความยากลงใน PlayerPrefs
        PlayerPrefs.SetInt("SelectedDifficulty", difficultyLevel);
        PlayerPrefs.Save();
    }

    // ฟังก์ชันสำหรับโหลดระดับความยาก
    public void LoadDifficulty()
    {
        // ตรวจสอบว่ามีการบันทึกความยากหรือไม่ และโหลดค่าเก่า
        if (PlayerPrefs.HasKey("SelectedDifficulty"))
        {
            GameSettings.difficultyLevel = PlayerPrefs.GetInt("SelectedDifficulty");
        }
        else
        {
            GameSettings.difficultyLevel = 0; // ตั้งค่าเริ่มต้นเป็นง่าย
        }
    }
}