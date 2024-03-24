using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer1 : MonoBehaviour
{
    [SerializeField] public float timeCount;
    [SerializeField] TextMeshProUGUI timeText;

    void Start()
    {
        timeCount = 0.0f; // ตั้งค่าเวลาเริ่มต้น
    }

    void Update()
    {
        timeCount += Time.deltaTime; // เพิ่มเวลา

        int seconds = Mathf.FloorToInt(timeCount);

        timeText.text = string.Format("{0:0}", seconds);
    }
}

