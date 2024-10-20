using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("GoodItem"))
        {
            SoundManager.instance.Play(SoundManager.SoundName.CorrectItem);
            ScoreManagerStage4.Instance.AddScore(1);
            Thrownable.Instance.EnableThrowingAgain(); // อนุญาตให้ปาใหม่ได้
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("BadItem"))
        {
            SoundManager.instance.Play(SoundManager.SoundName.WrongItem);
            ScoreManagerStage4.Instance.SubtractScore(1);
            Thrownable.Instance.EnableThrowingAgain(); // อนุญาตให้ปาใหม่ได้
            Destroy(gameObject);
        }
    }

    
}