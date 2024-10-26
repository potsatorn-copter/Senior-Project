using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Reset : MonoBehaviour
{
    public Button resetButton; // ปุ่มในหน้าสรุปคะแนน

    void Start()
    {
        // ผูกปุ่มให้รีเซ็ตข้อมูลและกลับไปหน้าเลือกระดับความยาก
        resetButton.onClick.AddListener(ResetAndReturnToDifficultySelect);
    }

    // เมธอดสำหรับรีเซ็ตข้อมูลและกลับไปหน้าเลือกระดับความยาก
    void ResetAndReturnToDifficultySelect()
    {
        // เรียกเมธอดรีเซ็ตข้อมูลเกม
        ResetGameData();

        // ส่งผู้เล่นกลับไปหน้าเลือกระดับความยาก
        SceneManager.LoadScene("MainMenu");
    }

    // ฟังก์ชันรีเซ็ตข้อมูลเกม
    void ResetGameData()
    {
        // รีเซ็ตข้อมูลซีน คะแนน และระดับความยาก
        PlayerPrefs.DeleteKey("LastScenePlayed");
        PlayerPrefs.DeleteKey("SelectedDifficulty");

        // เรียกฟังก์ชัน ClearScores() จาก ScoreManager เพื่อล้างคะแนนทั้งหมด
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ClearScores();
        }

        // บันทึกข้อมูลลง PlayerPrefs หลังการรีเซ็ต
        PlayerPrefs.Save();
    }
}