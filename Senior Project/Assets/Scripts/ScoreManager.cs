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
    }

    // ฟังก์ชันสำหรับคำนวณคะแนนรวม
    public int GetTotalScore()
    {
        return scoreScene1 + scoreScene2 + scoreScene3 + scoreScene4 + scoreScene5;
    }
}
