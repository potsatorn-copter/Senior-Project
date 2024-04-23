using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool IsGoodItem { get; private set; } // ตัวแปรสำหรับระบุว่าเป็นของดีหรือของเลว

    // ใช้เพื่อเริ่มต้นหรือรีเซ็ตไอเท็ม
    public void Initialize(bool isGood)
    {
        IsGoodItem = isGood;
        // ตั้งค่าเพิ่มเติมสำหรับไอเท็มที่นี่ ถ้าจำเป็น
    }

    // เรียกใช้เมื่อต้องการใช้งานไอเท็มนี้ในเกม
    public void Activate(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        // ตั้งค่าเพิ่มเติมสำหรับการเปิดใช้งาน
    }

    // เรียกใช้เมื่อไอเท็มนี้ถูกจับหรือไม่จำเป็นต้องใช้งานแล้ว
    public void Deactivate()
    {
        gameObject.SetActive(false);
        // รีเซ็ตสถานะของไอเท็มที่นี่ ถ้าจำเป็น
    }
}
