using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Itempool : MonoBehaviour
{
    public GameObject goodItemPrefab;
    public GameObject badItemPrefab;
    public static int poolSize = 18;  // ขนาดของ pool คือ 18 ชิ้น (12 ไอเท็มดี + 6 ไอเท็มไม่ดี)
    
    private static List<GameObject> goodItemsPool;
    private static List<GameObject> badItemsPool;
    private int itemsActivated = 0;
    private bool poolCompleted = false;

    private void Awake()
    {
        // สร้างรายการไอเท็มดีและไอเท็มไม่ดี
        goodItemsPool = new List<GameObject>(12);
        badItemsPool = new List<GameObject>(6);

        // เพิ่มไอเท็มดีเข้า pool
        for (int i = 0; i < 12; i++)
        {
            var goodItem = Instantiate(goodItemPrefab, Vector3.zero, Quaternion.identity);
            goodItem.SetActive(false);
            goodItemsPool.Add(goodItem);
        }

        // เพิ่มไอเท็มไม่ดีเข้า pool
        for (int i = 0; i < 6; i++)
        {
            var badItem = Instantiate(badItemPrefab, Vector3.zero, Quaternion.identity);
            badItem.SetActive(false);
            badItemsPool.Add(badItem);
        }
    }

    public GameObject GetItemFromPool()
    {
        if (poolCompleted) return null;

        float phase = (float)itemsActivated / poolSize;
        float goodItemChance = 1.0f;

        // ปรับอัตราส่วนการสุ่มไอเท็มตามเฟสของเกม
        if (phase < 0.33f)
        {
            goodItemChance = 0.65f;  // ช่วงแรก 75% เป็นไอเท็มดี
        }
        else if (phase < 0.66f)
        {
            goodItemChance = 0.55f;  // ช่วงกลาง 60% เป็นไอเท็มดี
        }
        else
        {
            goodItemChance = 0.50f;  // ช่วงท้าย 50/50 ระหว่างดีและไม่ดี
        }

        GameObject itemToSpawn;
        if (Random.value <= goodItemChance && goodItemsPool.Count > 0)
        {
            itemToSpawn = goodItemsPool[0];
            goodItemsPool.RemoveAt(0); // เอาไอเท็มออกจาก pool อย่างถาวร
        }
        else if (badItemsPool.Count > 0)
        {
            itemToSpawn = badItemsPool[0];
            badItemsPool.RemoveAt(0); // เอาไอเท็มออกจาก pool อย่างถาวร
        }
        else
        {
            return null;  // หากไม่มีไอเท็มเหลือ
        }

        itemToSpawn.SetActive(true);
        itemsActivated++;

        // เช็คว่าไอเท็มใน pool ถูกดึงออกครบแล้วหรือยัง
        if (itemsActivated >= poolSize)
        {
            poolCompleted = true;
        }

        return itemToSpawn;
    }

    public static void ReturnItemToPool(GameObject item)
    {
        item.SetActive(false);

        // เอาไอเท็มกลับเข้า pool ตามประเภทได้ หากต้องการในภายหลัง
        if (item.CompareTag("GoodItem") && goodItemsPool.Count < 12)
        {
            goodItemsPool.Add(item);
        }
        else if (item.CompareTag("BadItem") && badItemsPool.Count < 6)
        {
            badItemsPool.Add(item);
        }
    }

    // ฟังก์ชันเช็คว่า Pool หมดแล้วหรือยัง
    public bool IsPoolCompleted()
    {
        return poolCompleted;
    }
}