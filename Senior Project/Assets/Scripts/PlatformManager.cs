using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] GameObject platformPresetEasy;
    [SerializeField] GameObject platformPresetNormal;
    [SerializeField] GameObject platformPresetHard;


    void Start()
    {
        platformPresetEasy.SetActive(false);
        platformPresetNormal.SetActive(false);
        platformPresetHard.SetActive(false);

        // ตรวจสอบระดับความยาก
        if (GameSettings.difficultyLevel == 0) // ง่าย
        {
            platformPresetEasy.SetActive(true);
        }
        else if (GameSettings.difficultyLevel == 1) // กลาง
        {
            platformPresetNormal.SetActive(true);
        }
        else if (GameSettings.difficultyLevel == 2) // ยาก
        {
            platformPresetHard.SetActive(true);
        }
        else
        {
            // ถ้าไม่เจอค่า 0, 1, หรือ 2 ให้เปิด Easy เป็นค่าเริ่มต้น
            platformPresetEasy.SetActive(true);
        }
    }
}
