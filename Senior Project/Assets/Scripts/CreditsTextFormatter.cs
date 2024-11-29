using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreditsTextFormatter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI creditsText;

    void Start()
    {
        // จัดข้อความด้วยการใช้แท็บ (\t) เพื่อจัดระยะช่องว่างให้เท่ากันทุกบรรทัด
        string formattedText = 
                                     "\t\t\t\t\t\t\t\tจัดทำโดย\n" +
            "\tAlphaCore Studios\n" +
            "\tพสธร บุญล้อม\t\t\tGame Developer, Project Manager\n" +
            "\tพิริยะพงศ์ เพชรศิริ\t\t\tGame Developer\n" +
            "\tอธิเมศร์ สมบัติเลิศสิริ\t\tGame Developer\n" +
            "\tทิพณัฐ หมอกแก้ว\t\t\t3D 2D Artist\n" +
            "\tวรเมธ หมุนทรัพย์\t\t\t3D 2D Artist\n" +
            "\tวันชัย วงจันทร์\t\t\tGame Designer\n" +
            "\tภฤศ เกื้อหนุน\t\t\t\t\tGame Designer\n" +
            "\tปุญญพัฒน์ มหวนมาลิน\t\tGame Designer\n" 
                              ;

        // ตั้งค่าข้อความที่จัดรูปแบบแล้วไปยัง TextMeshProUGUI
        creditsText.text = formattedText;

        // ปรับระยะบรรทัดใน TextMeshProUGUI (ถ้าจำเป็น)
        creditsText.lineSpacing = 1.2f; // คุณสามารถปรับค่าได้ตามความเหมาะสม
        creditsText.paragraphSpacing = 10.65f;
    }
}
