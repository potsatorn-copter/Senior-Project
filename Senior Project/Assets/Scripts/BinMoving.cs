using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA; // จุดเริ่มต้น
    public Transform pointB; // จุดปลายทาง
    public float speed = 2f; // ความเร็วในการเคลื่อนที่
    private Vector3 target; // เป้าหมายการเคลื่อนที่ปัจจุบัน

    void Start()
    {
        target = pointB.position; // เริ่มต้นเคลื่อนที่ไปยังจุด B
    }

    void Update()
    {
        // เคลื่อนที่ไปยังเป้าหมาย
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // เมื่อถึงจุดหมายสลับเป้าหมายไปยังจุดอีกจุดหนึ่ง
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            // สลับเป้าหมาย
            target = (target == pointA.position) ? pointB.position : pointA.position;
        }
    }
}
