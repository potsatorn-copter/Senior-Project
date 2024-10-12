using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject platformPrefab; // Prefab ของแพลตฟอร์ม
    public int poolSize = 10; // จำนวนแพลตฟอร์มใน pool
    public float activationDistance = 20f; // ระยะทางจากผู้เล่น/กล้องที่แพลตฟอร์มจะเปิดใช้งาน
    public Transform player; // ตำแหน่งของผู้เล่นหรือกล้อง
    public float minX = -5f, maxX = 5f; // ช่วงตำแหน่ง X ที่แพลตฟอร์มจะถูกสร้าง
    public float distanceBetweenPlatforms = 3f; // ระยะห่างระหว่างแพลตฟอร์มแต่ละอัน

    private Queue<GameObject> platformPool = new Queue<GameObject>(); // Pool ของแพลตฟอร์ม
    private float nextPlatformY = 0f; // ตำแหน่ง Y ที่แพลตฟอร์มใหม่จะถูกสร้าง

    private void Start()
    {
        // สร้างแพลตฟอร์มทั้งหมดล่วงหน้าและใส่ลงใน pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject platform = Instantiate(platformPrefab);
            platform.SetActive(false); // ปิดแพลตฟอร์มตอนเริ่มเกม
            platformPool.Enqueue(platform);
        }

        // สร้างแพลตฟอร์มเริ่มต้นในเกม
        for (int i = 0; i < poolSize; i++)
        {
            SpawnPlatform();
        }
    }

    private void Update()
    {
        // ตรวจสอบแพลตฟอร์มที่ควรเปิดใช้งานใหม่ตามระยะของผู้เล่น
        if (player.position.y > nextPlatformY - poolSize * distanceBetweenPlatforms)
        {
            SpawnPlatform(); // สร้างแพลตฟอร์มใหม่เมื่อผู้เล่นขึ้นไปถึงระดับสูงขึ้น
        }

        // ตรวจสอบแพลตฟอร์มที่พ้นจากระยะกล้องและนำกลับไปใส่ใน pool
        foreach (GameObject platform in platformPool)
        {
            if (platform.activeInHierarchy && platform.transform.position.y < player.position.y - activationDistance * 2)
            {
                platform.SetActive(false); // ปิดแพลตฟอร์มที่พ้นระยะ
                platformPool.Enqueue(platform); // คืนแพลตฟอร์มกลับไปที่ pool เพื่อใช้งานใหม่
            }
        }
    }

    private void SpawnPlatform()
    {
        if (platformPool.Count > 0)
        {
            GameObject platform = platformPool.Dequeue(); // นำแพลตฟอร์มออกจาก pool
            float randomX = Random.Range(minX, maxX); // สุ่มตำแหน่ง X
            platform.transform.position = new Vector3(randomX, nextPlatformY, 0f); // ตั้งตำแหน่งใหม่
            platform.SetActive(true); // เปิดใช้งานแพลตฟอร์ม
            nextPlatformY += distanceBetweenPlatforms; // อัปเดตตำแหน่ง Y สำหรับแพลตฟอร์มถัดไป
        }
    }
}