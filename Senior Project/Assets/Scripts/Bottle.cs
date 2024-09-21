using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bottle : MonoBehaviour
{
    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D other) // แก้ไขให้ถูกต้อง
    {
        if (other.gameObject.CompareTag("GoodItem"))
        {
                SoundManager.instance.Play(SoundManager.SoundName.Correct);
                // เป็น GoodItem บวกคะแนน 1
                Debug.Log("This is Good Player");
                ScoreManagerStage8.Instance.AddScore(1);
                Destroy(gameObject);
                Debug.Log("+1 Score");
        }
        else if (other.gameObject.CompareTag("BadItem"))
        {
                SoundManager.instance.Play(SoundManager.SoundName.Wrong);
                // เป็น BadItem ลบคะแนน 1
                Debug.Log("This is Bad Player");
            
                ScoreManagerStage8.Instance.SubtractScore(1);
                Destroy(gameObject);
                Debug.Log("-1 Score");
        }
    }
}
