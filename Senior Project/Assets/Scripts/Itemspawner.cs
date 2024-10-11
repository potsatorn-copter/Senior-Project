using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Itemspawner : MonoBehaviour
{
    public Itempool itemPool;
    public Transform spawnPoint;
    public ScoreManager1 scoreManager; // อ้างอิงไปยัง ScoreManager เพื่อจบเกมเมื่อไอเท็มหมด

    private void Start()
    {
        StartCoroutine(SpawnItems());
    }

    private IEnumerator SpawnItems()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 2f));

            var itemGameObject = itemPool.GetItemFromPool();
            Item item = null;
            if (itemGameObject != null)
            {
                item = itemGameObject.GetComponent<Item>();
                if (item != null && spawnPoint != null)
                {
                    item.Activate(spawnPoint.position);
                }
                else
                {
                    Debug.LogError("Item or spawnPoint is null.");
                    Itempool.ReturnItemToPool(itemGameObject);
                }
            }

            // เช็คว่าไอเท็มใน Pool หมดหรือไม่ ถ้าหมดให้จบเกม
            if (itemPool.IsPoolCompleted())
            {
                yield return new WaitForSeconds(2.0f); // ดีเลย์ 2 วินาทีก่อนจบเกม
                scoreManager.CalculateFinalScore(); // เรียกใช้ฟังก์ชันคำนวณคะแนน
                break; // ออกจากลูปเมื่อไอเท็มหมด
            }

            yield return new WaitForSeconds(Random.Range(3f, 6f));

            if (item != null)
            {
                item.Deactivate();
                Itempool.ReturnItemToPool(itemGameObject);
            }
        }
    }
}