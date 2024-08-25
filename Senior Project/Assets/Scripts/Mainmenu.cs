using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Stage");
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundManager.SoundName.MainmenuSong);
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }

    public void QuitGame()
    {
        Application.Quit();
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }

    public void Stage1Rule()
    {
        SceneManager.LoadScene("Stage 1 Rule");
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }
    public void Stage1Play()
        {
            SceneManager.LoadScene("Stage 1 Play");
            Time.timeScale = 1;
            SoundManager.instance.Play(SoundManager.SoundName.Click);
        }
         public void Stage2Rule()
            {
                SceneManager.LoadScene("Stage 2 Rule");
                Time.timeScale = 1;
                SoundManager.instance.Play(SoundManager.SoundName.Click);
            }
            public void Stage2Play()
                {
                    SceneManager.LoadScene("Stage 2 Play");
                    Time.timeScale = 1;
                    SoundManager.instance.Play(SoundManager.SoundName.Click);
                }
        
        
    public void Stage3Rule()
    
    {
            SceneManager.LoadScene("Stage 3 Rule");
            Time.timeScale = 1;
            SoundManager.instance.Play(SoundManager.SoundName.Click);
    }
        
    public void Stage4Rule()
    {
        SceneManager.LoadScene("Stage 4 Rule");
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }
    
    public void Stage4Play()
        {
            SceneManager.LoadScene("Stage 4 Play");
            Time.timeScale = 1;
            SoundManager.instance.Play(SoundManager.SoundName.Click);
        }

  
   
    
}
