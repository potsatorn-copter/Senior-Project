using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RulePageController : MonoBehaviour
{
    public GameObject[] rulePages; // เก็บหน้ากฏแต่ละหน้าในรูปแบบ GameObject
    public Button nextButton; // ปุ่มถัดไป
    public Button previousButton; // ปุ่มย้อนกลับ
    private int currentPageIndex = 0; // ตัวแปรเก็บหน้าปัจจุบัน

    void Start()
    {
        ShowPage(currentPageIndex);

        // เพิ่มฟังก์ชันเมื่อกดปุ่ม
        nextButton.onClick.AddListener(NextPage);
        previousButton.onClick.AddListener(PreviousPage);

        // ปิดปุ่มย้อนกลับเมื่ออยู่หน้าแรก
        previousButton.interactable = false;
    }

    // ฟังก์ชันแสดงหน้า
    void ShowPage(int index)
    {
        // ปิดทุกหน้า
        foreach (GameObject page in rulePages)
        {
            page.SetActive(false);
        }
        // แสดงหน้าที่ต้องการ
        rulePages[index].SetActive(true);
        
        // จัดการปุ่มย้อนกลับ
        previousButton.interactable = index > 0;
        // จัดการปุ่มถัดไป
        nextButton.interactable = index < rulePages.Length - 1;
    }

    // ฟังก์ชันเมื่อกดปุ่มถัดไป
    public void NextPage()
    {
        if (currentPageIndex < rulePages.Length - 1)
        {
            currentPageIndex++;
            ShowPage(currentPageIndex);
        }
    }

    // ฟังก์ชันเมื่อกดปุ่มย้อนกลับ
    public void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            ShowPage(currentPageIndex);
        }
    }
}
