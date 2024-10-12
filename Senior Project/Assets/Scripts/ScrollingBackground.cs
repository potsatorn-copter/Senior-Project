using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public Transform player; // ตัวละครผู้เล่น
    public float scrollSpeed = 0.1f; // ความเร็วในการเลื่อนพื้นหลัง
    public float backgroundHeight; // ความสูงของภาพพื้นหลัง
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position; // ตำแหน่งเริ่มต้นของพื้นหลัง
    }

    private void Update()
    {
        // ตรวจสอบว่าผู้เล่นไปถึงตำแหน่งสูงสุดของภาพพื้นหลังแล้วหรือยัง
        if (player.position.y > startPosition.y + backgroundHeight)
        {
            // เริ่มเลื่อนพื้นหลังลงมาเมื่อผู้เล่นไปถึงจุดสูงสุดของภาพพื้นหลัง
            Vector3 newPosition = transform.position;
            newPosition.y -= backgroundHeight; // เลื่อนพื้นหลังลงมาเริ่มต้นใหม่จากส่วนล่างของภาพต่อไป
            transform.position = newPosition;

            // รีเซ็ตตำแหน่งเริ่มต้นใหม่
            startPosition = transform.position;
        }
    }
}