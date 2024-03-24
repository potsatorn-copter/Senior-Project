using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] public float timeCount;
    [SerializeField] TextMeshProUGUI timeText;
   
        void Update()
        {
            if (timeCount > 0)
            {
                timeCount -= Time.deltaTime;
            }
            else if (timeCount < 0)
            {
                timeCount = 0;
            }

            int seconds = Mathf.FloorToInt(timeCount);

            timeText.text = string.Format("{00:00}", seconds);
        }
}
