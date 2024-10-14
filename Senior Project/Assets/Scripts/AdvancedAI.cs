using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedAI : MonoBehaviour
{
    public float delayBeforeReturning = 0.1f;  // ลดเวลา delay ให้เร็วขึ้น
    public float movementSpeed = 2f;  // ปรับความเร็วให้เคลื่อนที่เร็วกว่า
    public LayerMask itemLayerMask;
    public float viewRadius = 12f;  // ปรับระยะการมองเห็นให้กว้างกว่า
    [Range(0, 360)]
    public float viewAngle = 120f;  // มุมมองกว้างกว่า

    private Vector3 startPosition;
    private float nextMoveTime = 0f;
    private Transform closestItemTransform = null;
    private bool isMoving = false;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (Time.time >= nextMoveTime && !isMoving)
        {
            FindClosestItem();
            if (closestItemTransform != null)
            {
                StartCoroutine(MoveToTargetPosition(closestItemTransform.position));
            }
        }
    }

    // ค้นหาเฉพาะ GoodItem
    void FindClosestItem()
    {
        Collider[] itemsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius);
        float closestDistance = Mathf.Infinity;
        Collider closestItem = null;

        foreach (Collider item in itemsInViewRadius)
        {
            // ตรวจสอบว่าไอเท็มมี Tag เป็น GoodItem เท่านั้น
            if (item.CompareTag("GoodItem"))
            {
                float distanceToItem = Vector3.Distance(transform.position, item.transform.position);
                if (distanceToItem < closestDistance && item.gameObject.activeInHierarchy)
                {
                    closestDistance = distanceToItem;
                    closestItem = item;
                }
            }
        }

        closestItemTransform = closestItem != null ? closestItem.transform : null;
    }

    IEnumerator MoveToTargetPosition(Vector3 targetPos)
    {
        isMoving = true;

        yield return new WaitForSeconds(delayBeforeReturning);  // Delay สั้นลงเพราะ AI เก่งขึ้น

        float elapsedTime = 0;
        Vector3 originalPosition = transform.position;
        while (elapsedTime < movementSpeed)
        {
            if (closestItemTransform == null || !closestItemTransform.gameObject.activeInHierarchy)
            {
                break;
            }

            transform.position = Vector3.Lerp(originalPosition, targetPos, elapsedTime / movementSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = startPosition;  // กลับสู่ตำแหน่งเริ่มต้น
        isMoving = false;
        closestItemTransform = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ตรวจสอบว่าเป็น GoodItem
        if (other.gameObject.CompareTag("GoodItem"))
        {
            Debug.Log("Advanced AI collected a GoodItem");

            // คืนไอเท็มกลับสู่ pool
            Item itemScript = other.GetComponent<Item>();
            if (itemScript != null)
            {
                itemScript.Deactivate();
                Itempool.ReturnItemToPool(other.gameObject);
            }
        }
    }

    void OnDrawGizmos()
    {
        // แสดงรัศมีการมองเห็นของ AI
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 forwardYAdjusted = transform.forward + Vector3.up * 0.5f;
        Vector3 frontRayPoint = transform.position + (forwardYAdjusted.normalized * viewRadius);
        Vector3 leftRayPoint = transform.position + (Quaternion.Euler(0, -viewAngle / 2, 0) * forwardYAdjusted.normalized * viewRadius);
        Vector3 rightRayPoint = transform.position + (Quaternion.Euler(0, viewAngle / 2, 0) * forwardYAdjusted.normalized * viewRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, frontRayPoint);
        Gizmos.DrawLine(transform.position, leftRayPoint);
        Gizmos.DrawLine(transform.position, rightRayPoint);

        if (closestItemTransform != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, closestItemTransform.position);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(startPosition, 0.2f);
    }
}
