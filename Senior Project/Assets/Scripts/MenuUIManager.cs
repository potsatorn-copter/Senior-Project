using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    public Image unlockNotificationUI; // UI สำหรับการแจ้งเตือนปลดล็อค

    private void OnEnable()
    {
        if (unlockNotificationUI != null)
        {
            unlockNotificationUI.gameObject.SetActive(false); // ซ่อน UI ตั้งแต่เริ่มต้น
        }

        CheckUnlockedImages();
    }

    private void CheckUnlockedImages()
    {
        bool hasUnlockedImages = false;

        // ตรวจสอบภาพทั้งหมดว่าอันไหนปลดล็อคแล้วแต่ยังไม่ดู
        string[] imageNames = { "ImageEasy", "ImageNormal", "ImageHard" };
        foreach (string imageName in imageNames)
        {
            if (PlayerPrefs.GetInt("ImageUnlocked_" + imageName, 0) == 1)
            {
                hasUnlockedImages = true;
                break;
            }
        }

        // แสดง Notification UI ถ้ามีภาพที่ปลดล็อค
        if (hasUnlockedImages && unlockNotificationUI != null)
        {
            unlockNotificationUI.gameObject.SetActive(true);
            Debug.Log("Notification displayed: There are unlocked images");
        }
        else
        {
            Debug.Log("No unlocked images to display notification.");
        }
    }

    public void OnGalleryOpened()
    {
        if (unlockNotificationUI != null)
        {
            unlockNotificationUI.gameObject.SetActive(false); // ซ่อน UI เมื่อเข้าแกลลอรี
        }

        // รีเซ็ตสถานะการปลดล็อคใน PlayerPrefs เมื่อผู้เล่นเข้าแกลลอรี
        string[] imageNames = { "ImageEasy", "ImageNormal", "ImageHard" };
        foreach (string imageName in imageNames)
        {
            PlayerPrefs.SetInt("ImageUnlocked_" + imageName, 0);
            Debug.Log("Resetting unlock state for: " + imageName);
        }
        PlayerPrefs.Save();
    }
}