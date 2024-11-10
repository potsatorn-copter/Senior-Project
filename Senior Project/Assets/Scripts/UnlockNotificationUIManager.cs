using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockNotificationUIManager : MonoBehaviour
{
    public GameObject easyNotification;   // GameObject สำหรับแจ้งเตือน Easy
    public GameObject normalNotification; // GameObject สำหรับการแจ้งเตือน Normal
    public GameObject hardNotification;   // GameObject สำหรับการแจ้งเตือน Hard

    void OnEnable()
    {
        // ปิดการแสดงผลของการแจ้งเตือนทั้งหมดไว้ก่อน
        easyNotification.SetActive(false);
        normalNotification.SetActive(false);
        hardNotification.SetActive(false);

        // ตรวจสอบสถานะการปลดล็อคภาพจาก PlayerPrefs
        Debug.Log("Starting Unlock Notification Check...");
        CheckUnlockNotifications();
    }

    private void CheckUnlockNotifications()
    {
        bool isEasyUnlocked = PlayerPrefs.GetInt("isEasyUnlocked", 0) == 1;
        bool hasShownEasyNotification = PlayerPrefs.GetInt("hasShownEasyUnlockNotification", 0) == 1;

        bool isNormalUnlocked = PlayerPrefs.GetInt("isNormalUnlocked", 0) == 1;
        bool hasShownNormalNotification = PlayerPrefs.GetInt("hasShownNormalUnlockNotification", 0) == 1;

        bool isHardUnlocked = PlayerPrefs.GetInt("isHardUnlocked", 0) == 1;
        bool hasShownHardNotification = PlayerPrefs.GetInt("hasShownHardUnlockNotification", 0) == 1;

        Debug.Log($"PlayerPrefs - isEasyUnlocked: {isEasyUnlocked}, hasShownEasyUnlockNotification: {hasShownEasyNotification}");
        Debug.Log($"PlayerPrefs - isNormalUnlocked: {isNormalUnlocked}, hasShownNormalUnlockNotification: {hasShownNormalNotification}");
        Debug.Log($"PlayerPrefs - isHardUnlocked: {isHardUnlocked}, hasShownHardUnlockNotification: {hasShownHardNotification}");

        if (isEasyUnlocked && !hasShownEasyNotification)
        {
            ShowNotification(easyNotification, "Easy");
        }
        else
        {
            Debug.Log("Easy notification not shown.");
        }

        if (isNormalUnlocked && !hasShownNormalNotification)
        {
            ShowNotification(normalNotification, "Normal");
        }
        else
        {
            Debug.Log("Normal notification not shown.");
        }

        if (isHardUnlocked && !hasShownHardNotification)
        {
            ShowNotification(hardNotification, "Hard");
        }
        else
        {
            Debug.Log("Hard notification not shown.");
        }
    }

    private void ShowNotification(GameObject notification, string level)
    {
        // ระบุการเปิดใช้งานรายตัวสำหรับการแจ้งเตือนแต่ละประเภท
        if (level == "Easy")
        {
            easyNotification.SetActive(true);
            Debug.Log("Easy notification set to active: " + easyNotification.activeSelf);
        }
        else if (level == "Normal")
        {
            normalNotification.SetActive(true);
            Debug.Log("Normal notification set to active: " + normalNotification.activeSelf);
        }
        else if (level == "Hard")
        {
            hardNotification.SetActive(true);
            Debug.Log("Hard notification set to active: " + hardNotification.activeSelf);
        }

        // Debug เพิ่มเติมเพื่อยืนยันว่า `easyNotification` ยังคงแสดงผลอยู่
        Debug.Log("Confirming visibility - " + level + "Notification activeSelf: " + notification.activeSelf + ", activeInHierarchy: " + notification.activeInHierarchy);
    }

    public void HideAllNotifications()
    {
        bool hasUpdatedPrefs = false;

        if (easyNotification.activeSelf)
        {
            PlayerPrefs.SetInt("hasShownEasyUnlockNotification", 1);
            Debug.Log("PlayerPrefs updated - hasShownEasyUnlockNotification set to 1");
            hasUpdatedPrefs = true;
        }

        if (normalNotification.activeSelf)
        {
            PlayerPrefs.SetInt("hasShownNormalUnlockNotification", 1);
            Debug.Log("PlayerPrefs updated - hasShownNormalUnlockNotification set to 1");
            hasUpdatedPrefs = true;
        }

        if (hardNotification.activeSelf)
        {
            PlayerPrefs.SetInt("hasShownHardUnlockNotification", 1);
            Debug.Log("PlayerPrefs updated - hasShownHardUnlockNotification set to 1");
            hasUpdatedPrefs = true;
        }

        easyNotification.SetActive(false);
        normalNotification.SetActive(false);
        hardNotification.SetActive(false);
        Debug.Log("All notifications hidden.");

        if (hasUpdatedPrefs)
        {
            PlayerPrefs.Save();
            Debug.Log("PlayerPrefs changes saved.");
        }
    }
}
