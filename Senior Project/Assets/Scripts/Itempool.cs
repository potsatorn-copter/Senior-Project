using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itempool : MonoBehaviour
{
    public GameObject goodItemPrefab;
    public GameObject badItemPrefab;
    public int poolSize = 10;

    private static Queue<GameObject> itemsPool;

    private void Awake()
    {
        // สร้าง pool ของไอเท็ม
        itemsPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            // สร้างของดีและของเลวแบบสุ่ม
            var prefab = Random.value > 0.5f ? goodItemPrefab : badItemPrefab;
            var newItem = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newItem.SetActive(false);
            itemsPool.Enqueue(newItem);
        }
    }

    public GameObject GetItemFromPool()
    {
        if (itemsPool.Count > 0)
        {
            var item = itemsPool.Dequeue();
            item.SetActive(true);
            return item;
        }
        else
        {
            // สร้างของดีและของเลวแบบสุ่ม
            var prefab = Random.value > 0.5f ? goodItemPrefab : badItemPrefab;
            var newItem = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            return newItem;
        }
    }

    public static void ReturnItemToPool(GameObject item)
    {
        item.SetActive(false);
        itemsPool.Enqueue(item);
    }
}
