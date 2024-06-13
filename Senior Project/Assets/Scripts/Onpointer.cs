using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onpointer : MonoBehaviour
{

    public void OnPointerEnter()
    {
        {
            SoundManager.instance.Play(SoundManager.SoundName.hoverSound);
        }
    }
}
