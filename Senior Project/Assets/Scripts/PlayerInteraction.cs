using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public CupManager cupManager;

    void Update()
    {
        if (cupManager.IsShuffling())
            return; // ถ้ายังอยู่ในขั้นตอนการสลับ ให้ข้ามการตรวจจับคลิกและสัมผัส

        // สำหรับการคลิกเม้าส์
        if (Input.GetMouseButtonDown(0)) 
        {
            HandleInput(Input.mousePosition);
        }

        // สำหรับการสัมผัสบนมือถือ
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                HandleInput(touch.position);
            }
        }
    }

    // ฟังก์ชันสำหรับการจัดการตำแหน่งการสัมผัสและการคลิก
    void HandleInput(Vector3 inputPosition)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(inputPosition);

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("คลิกถูกต้องที่: " + hit.transform.name); 

            for (int i = 0; i < cupManager.cups.Length; i++)
            {
                if (hit.transform == cupManager.cups[i].transform)
                {
                    Debug.Log("คลิกที่ถ้วย: " + i);
                    cupManager.CheckCup(i); 
                    break;
                }
            }
        }
        else
        {
            Debug.Log("ไม่ได้คลิกบนถ้วย");
        }
    }
}