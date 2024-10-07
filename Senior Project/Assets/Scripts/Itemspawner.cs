using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itemspawner : MonoBehaviour
{
    public Itempool itemPool;
    public Transform spawnPoint;

    private void Start()
    {
        StartCoroutine(SpawnItems());
    }

    private IEnumerator SpawnItems()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f,2f)); // รอเวลาสุ่ม

            var itemGameObject = itemPool.GetItemFromPool();
            Item item = null; // ประกาศ item ที่นี่เพื่อให้สามารถใช้ได้ทั่วทั้ง scope ของ coroutine
            if (itemGameObject != null) // ตรวจสอบว่าได้ GameObject จาก pool มาหรือไม่
            {
                item = itemGameObject.GetComponent<Item>(); // พยายามดึงคอมโพเนนต์ Item
                if (item != null && spawnPoint != null) // ตรวจสอบว่าทั้ง item และ spawnPoint ไม่เป็น null
                {
                    item.Activate(spawnPoint.position); // เปิดใช้งานไอเท็มที่ตำแหน่งเกิด
                }
                else
                {
                    Debug.LogError("Item or spawnPoint is null.");
                    Itempool.ReturnItemToPool(itemGameObject); // คืน GameObject กลับสู่ pool ถ้าไม่สามารถเปิดใช้งานได้
                }
            }

            yield return new WaitForSeconds(Random.Range(3f, 6f)); // ไอเท็มจะอยู่ใน scene ระหว่าง 5 ถึง 10 วินาที

            // ตรวจสอบค่า null อีกครั้งก่อนการปิดใช้งาน
            if (item != null) // ตอนนี้ item สามารถใช้ได้เพราะถูกประกาศอยู่นอกบล็อค if
            {
                item.Deactivate();
                Itempool.ReturnItemToPool(itemGameObject); // คืนไอเท็มกลับสู่ pool
            }
        }
    }
}
