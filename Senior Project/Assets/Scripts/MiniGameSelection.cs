using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public void LoadMiniGame(int miniGameNumber)
    {
        switch (miniGameNumber)
        {
            case 1:
                SceneManager.LoadScene("Stage 1 Rule"); // เปลี่ยนเป็นชื่อจริงของ Scene
                break;
            case 2:
                SceneManager.LoadScene("Stage 2 Rule");
                break;
            case 3:
                SceneManager.LoadScene("Stage 4 Rule");
                break;
            case 4:
                SceneManager.LoadScene("Stage 8 Rule");
                break;
            case 5:
                SceneManager.LoadScene("Stage 3 Rule");
                break;
        }
    }
}

