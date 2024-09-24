using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public CupManager cupManager;

    void Update()
    {
        if (cupManager.IsShuffling())
            return; // ถ้ายังอยู่ในขั้นตอนการสลับ ให้ข้ามการตรวจจับคลิก

        if (Input.GetMouseButtonDown(0)) // ตรวจสอบเมื่อผู้เล่นคลิกเมาส์
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) // ใช้ raycast ตรวจสอบว่าคลิกบนถ้วยหรือไม่
            {
                Debug.Log("คลิกถูกต้องที่: " + hit.transform.name); // แสดงชื่อของวัตถุที่ถูกคลิก

                for (int i = 0; i < cupManager.cups.Length; i++)
                {
                    if (hit.transform == cupManager.cups[i].transform)
                    {
                        Debug.Log("คลิกที่ถ้วย: " + i);
                        cupManager.CheckCup(i); // เรียกฟังก์ชัน CheckCup เมื่อคลิกที่ถ้วย
                        break; // หยุดการตรวจสอบเมื่อพบถ้วยที่ถูกคลิก
                    }
                }
            }
            else
            {
                Debug.Log("ไม่ได้คลิกบนถ้วย");
            }
        }
    }
}