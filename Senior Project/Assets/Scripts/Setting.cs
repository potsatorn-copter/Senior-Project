using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public GameObject settingPanel;
    
    public void ToggleSettings()
    {
        SoundManager.instance.Play(SoundManager.SoundName.Click);
        // สลับสถานะของ settingPanel
        settingPanel.SetActive(!settingPanel.activeSelf);

        // ตรวจสอบสถานะของ settingPanel และตั้งค่า Time.timeScale ตามนั้น
        if (settingPanel.activeSelf)
        {
            Time.timeScale = 0; // หยุดเวลาในเกมเมื่อ settingPanel ถูกแสดง
        }
        else
        {
            Time.timeScale = 1; // กลับค่าเวลาให้เป็นปกติเมื่อ settingPanel ถูกซ่อน
        }
    }
}
