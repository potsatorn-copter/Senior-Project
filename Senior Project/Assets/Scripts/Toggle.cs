using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle : MonoBehaviour
{
    public SoundManager soundManager; // Reference to the SoundManager

    public void ToggleSound() 
    {
        soundManager.MuteSound();
    }
    
}
