using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupManager : MonoBehaviour
{
    public GameObject[] cups; // อาเรย์ของถ้วย
    public GameObject ball; // ลูกบอล
    private int ballPosition; // ตำแหน่งถ้วยที่ซ่อนลูกบอล
    private bool shuffling = false; // กำลังสลับอยู่หรือไม่

    void Start()
    {
        ShuffleCups();
    }

    void ShuffleCups()
    {
        ballPosition = Random.Range(0, cups.Length);
        cups[ballPosition].transform.position = ball.transform.position;
    }

    public IEnumerator ShuffleAnimation()
    {
        shuffling = true;
        for (int i = 0; i < 10; i++)
        {
            int cupA = Random.Range(0, cups.Length);
            int cupB;
            do {
                cupB = Random.Range(0, cups.Length);
            } while (cupA == cupB);

            Vector3 tempPosition = cups[cupA].transform.position;
            cups[cupA].transform.position = cups[cupB].transform.position;
            cups[cupB].transform.position = tempPosition;

            yield return new WaitForSeconds(0.5f); // รอระหว่างการสลับ
        }
        shuffling = false;
    }

    public bool IsShuffling()
    {
        return shuffling;
    }

    public int GetBallPosition()
    {
        return ballPosition;
    }
}

