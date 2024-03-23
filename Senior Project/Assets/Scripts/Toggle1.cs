using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle1 : MonoBehaviour
{
    public SoundManager soundManager; // Reference to the SoundManager
    
    public void ToggleSong() 
    {
        soundManager.MuteSong();
    }
    
}
