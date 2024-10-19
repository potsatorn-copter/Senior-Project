using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level");
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundManager.SoundName.MainmenuSong);
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }
    public void DoTest()
    { 
        SceneManager.LoadScene("MMSB FORM");
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundManager.SoundName.MainmenuSong);
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }

    public void SelectGame()
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
    public void ScoreAmount()
    {
        SceneManager.LoadScene("Score Amount");
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }

    public void Stage1Rule()
    {
        PlayerPrefs.SetInt("LastScenePlayed", SceneManager.GetActiveScene().buildIndex); 
        PlayerPrefs.Save();
        SceneManager.LoadScene("Stage 1 Rule");
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }
    public void Stage1Play()
    {
        PlayerPrefs.SetInt("LastScenePlayed", SceneManager.GetActiveScene().buildIndex); 
        PlayerPrefs.Save();
        SceneManager.LoadScene("Stage 1 Play");
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }
    public void Stage2Rule()
    {
        PlayerPrefs.SetInt("LastScenePlayed", SceneManager.GetActiveScene().buildIndex); 
        PlayerPrefs.Save();
        SceneManager.LoadScene("Stage 2 Rule");
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    } 
    public void Stage2Play()
    {
        PlayerPrefs.SetInt("LastScenePlayed", SceneManager.GetActiveScene().buildIndex); 
        PlayerPrefs.Save();
        SceneManager.LoadScene("Stage 2 Play");
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }

    public void Stage3Rule()
    {
        PlayerPrefs.SetInt("LastScenePlayed", SceneManager.GetActiveScene().buildIndex); 
        PlayerPrefs.Save();
        SceneManager.LoadScene("Stage 3 Rule");
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }

    public void Stage8Rule()
    {
        SceneManager.LoadScene("Stage 8 Rule");
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }

    public void Stage8Play()
    {
        SceneManager.LoadScene("Stage 8 Play");
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }
    
    public void Stage3Play()
    {
        PlayerPrefs.SetInt("LastScenePlayed", SceneManager.GetActiveScene().buildIndex); 
        PlayerPrefs.Save();
        SceneManager.LoadScene("Stage 3 Play");
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }
    public void Stage4Rule()
    {
        PlayerPrefs.SetInt("LastScenePlayed", SceneManager.GetActiveScene().buildIndex); 
        PlayerPrefs.Save();
        SceneManager.LoadScene("Stage 4 Rule");
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }
    public void Stage4Play()
    {
        PlayerPrefs.SetInt("LastScenePlayed", SceneManager.GetActiveScene().buildIndex); 
        PlayerPrefs.Save();
        SceneManager.LoadScene("Stage 4 Play");
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }
    public void Stage5Rule()
    {
        PlayerPrefs.SetInt("LastScenePlayed", SceneManager.GetActiveScene().buildIndex); 
        PlayerPrefs.Save();
        SceneManager.LoadScene("Stage 5 Rule");
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }
    public void Stage5Play()
    {
        PlayerPrefs.SetInt("LastScenePlayed", SceneManager.GetActiveScene().buildIndex); 
        PlayerPrefs.Save();
        SceneManager.LoadScene("Stage 5 Play");
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }
    
    
}
