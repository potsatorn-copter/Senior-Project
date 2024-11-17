using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockNotificationUIManager : MonoBehaviour
{
    public GameObject easyNotification;   // GameObject สำหรับแจ้งเตือน Easy
    public GameObject normalNotification; // GameObject สำหรับการแจ้งเตือน Normal
    public GameObject hardNotification;   // GameObject สำหรับการแจ้งเตือน Hard

    public void OnEnable()
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
        bool isEasyCompleted = PlayerPrefs.GetInt("isEasyCompleted", 0) == 1;
        bool isEasyUnlocked = PlayerPrefs.GetInt("isEasyUnlocked", 0) == 1;
        bool hasShownEasyNotification = PlayerPrefs.GetInt("hasShownEasyUnlockNotification", 0) == 1;

        bool isNormalCompleted = PlayerPrefs.GetInt("isNormalCompleted", 0) == 1;
        bool isNormalUnlocked = PlayerPrefs.GetInt("isNormalUnlocked", 0) == 1;
        bool hasShownNormalNotification = PlayerPrefs.GetInt("hasShownNormalUnlockNotification", 0) == 1;

        bool isHardCompleted = PlayerPrefs.GetInt("isHardCompleted", 0) == 1;
        bool isHardUnlocked = PlayerPrefs.GetInt("isHardUnlocked", 0) == 1;
        bool hasShownHardNotification = PlayerPrefs.GetInt("hasShownHardUnlockNotification", 0) == 1;

        Debug.Log($"PlayerPrefs - isEasyCompleted: {isEasyCompleted}, isEasyUnlocked: {isEasyUnlocked}, hasShownEasyUnlockNotification: {hasShownEasyNotification}");
        Debug.Log($"PlayerPrefs - isNormalCompleted: {isNormalCompleted}, isNormalUnlocked: {isNormalUnlocked}, hasShownNormalUnlockNotification: {hasShownNormalNotification}");
        Debug.Log($"PlayerPrefs - isHardCompleted: {isHardCompleted}, isHardUnlocked: {isHardUnlocked}, hasShownHardUnlockNotification: {hasShownHardNotification}");

        if (isEasyCompleted && !isEasyUnlocked && !hasShownEasyNotification)
        {
            ShowNotification(easyNotification, "Easy");
        }

        if (isNormalCompleted && !isNormalUnlocked && !hasShownNormalNotification)
        {
            ShowNotification(normalNotification, "Normal");
        }

        if (isHardCompleted && !isHardUnlocked && !hasShownHardNotification)
        {
            ShowNotification(hardNotification, "Hard");
        }
    }

    private void ShowNotification(GameObject notification, string level)
    {
        // แสดงการแจ้งเตือนตามประเภท
        notification.SetActive(true);
        Debug.Log($"{level} notification set to active.");
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