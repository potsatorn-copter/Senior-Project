using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public CupManager cupManager;

    void Update()
    {
        if (!cupManager.IsShuffling() && Input.GetMouseButtonDown(0)) // เมื่อคลิกเมาส์
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                for (int i = 0; i < cupManager.cups.Length; i++)
                {
                    if (hit.transform == cupManager.cups[i].transform)
                    {
                        CheckCup(i); // ตรวจสอบถ้วยที่คลิก
                    }
                }
            }
        }
    }

    void CheckCup(int index)
    {
        if (index == cupManager.GetBallPosition())
        {
            Debug.Log("คุณทายถูก! ลูกบอลอยู่ใต้ถ้วยนี้");
        }
        else
        {
            Debug.Log("คุณทายผิด! ลองใหม่");
        }
    }
}
