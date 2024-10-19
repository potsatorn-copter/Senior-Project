using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int scoreScene1 = 0;
    public int scoreScene2 = 0;
    public int scoreScene3 = 0;
    public int scoreScene4 = 0;
    public int scoreScene5 = 0;

    private void Awake()
    {
        // ทำให้ ScoreManager คงอยู่ข้ามซีน (DontDestroyOnLoad)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // ทำลายตัวที่ซ้ำ
        }

        // โหลดคะแนนจาก PlayerPrefs เมื่อเริ่มเกมใหม่
        LoadScores();
    }

    // ฟังก์ชันสำหรับตั้งค่าคะแนนในแต่ละซีน
    public void SetScoreForScene(int sceneIndex, int score)
    {
        switch (sceneIndex)
        {
            case 1:
                scoreScene1 = score;
                break;
            case 2:
                scoreScene2 = score;
                break;
            case 3:
                scoreScene3 = score;
                break;
            case 4:
                scoreScene4 = score;
                break;
            case 5:
                scoreScene5 = score;
                break;
        }

        // บันทึกคะแนนลงใน PlayerPrefs ทุกครั้งที่อัปเดตคะแนน
        SaveScores();
    }

    // ฟังก์ชันสำหรับคำนวณคะแนนรวม
    public int GetTotalScore()
    {
        return scoreScene1 + scoreScene2 + scoreScene3 + scoreScene4 + scoreScene5;
    }

    // ฟังก์ชันบันทึกคะแนนลงใน PlayerPrefs
    public void SaveScores()
    {
        PlayerPrefs.SetInt("ScoreScene1", scoreScene1);
        PlayerPrefs.SetInt("ScoreScene2", scoreScene2);
        PlayerPrefs.SetInt("ScoreScene3", scoreScene3);
        PlayerPrefs.SetInt("ScoreScene4", scoreScene4);
        PlayerPrefs.SetInt("ScoreScene5", scoreScene5);
        PlayerPrefs.Save();
    }

    // ฟังก์ชันโหลดคะแนนจาก PlayerPrefs
    public void LoadScores()
    {
        scoreScene1 = PlayerPrefs.GetInt("ScoreScene1", 0);
        scoreScene2 = PlayerPrefs.GetInt("ScoreScene2", 0);
        scoreScene3 = PlayerPrefs.GetInt("ScoreScene3", 0);
        scoreScene4 = PlayerPrefs.GetInt("ScoreScene4", 0);
        scoreScene5 = PlayerPrefs.GetInt("ScoreScene5", 0);
    }

    // ฟังก์ชันเคลียร์คะแนนทั้งหมดใน PlayerPrefs
    public void ClearScores()
    {
        PlayerPrefs.DeleteKey("ScoreScene1");
        PlayerPrefs.DeleteKey("ScoreScene2");
        PlayerPrefs.DeleteKey("ScoreScene3");
        PlayerPrefs.DeleteKey("ScoreScene4");
        PlayerPrefs.DeleteKey("ScoreScene5");
    }
}