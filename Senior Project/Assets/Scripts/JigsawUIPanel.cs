using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JigsawUIPanel : MonoBehaviour
{
    public Image panel;               // Panel ของ UI ที่จะแสดงเมื่อได้รับชิ้นส่วน
    public Image jigsawPieceImage;    // รูปภาพชิ้นส่วนจิ๊กซอว์
    public TextMeshProUGUI messageText; // ข้อความแจ้งเตือน

    private void Start()
    {
        // ซ่อน Panel เมื่อเริ่มเกม
        if (panel != null)
        {
            panel.gameObject.SetActive(false);
        }
    }

    // ฟังก์ชันที่ใช้ในการแสดง JigsawUIPanel พร้อมชิ้นส่วนและข้อความ
    public void ShowJigsawUIPanel(Sprite jigsawSprite, string message)
    {
        if (panel == null || jigsawPieceImage == null || messageText == null)
        {
            Debug.LogWarning("JigsawUIPanel is not set up correctly. Check panel, jigsawPieceImage, or messageText references.");
            return;
        }

        jigsawPieceImage.sprite = jigsawSprite; // ตั้งค่ารูปภาพของชิ้นส่วน
        messageText.text = message;             // ตั้งค่าข้อความแจ้งเตือน

        // แสดง Panel
        panel.gameObject.SetActive(true);

        // เรียก Coroutine เพื่อซ่อน UI หลังจากแสดงผลเสร็จ
        StartCoroutine(HideJigsawUIPanelAfterDelay());
    }

    // Coroutine สำหรับซ่อน UI หลังจากแสดงผล 3 วินาที
    public IEnumerator HideJigsawUIPanelAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        if (panel != null)
        {
            panel.gameObject.SetActive(false);
        }
    }
    public void HideJigsawUIPanel()
    {
        if (panel != null)
        {
            panel.gameObject.SetActive(false);
        }
    }
}