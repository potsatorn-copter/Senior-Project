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
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }
    public void DoTest()
    { 
        SceneManager.LoadScene("MMSB FORM");
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }

    public void SelectGame()
    {
        SceneManager.LoadScene("Stage");
        Time.timeScale = 1;
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
    
    public void GotoGallery()
    {
        SceneManager.LoadScene("MemoryGallery");
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundManager.SoundName.Click);

        // เรียกฟังก์ชัน UpdateGallery เพื่ออัปเดตภาพในแกลลอรี
        if (JigsawManager.Instance != null)
        {
            JigsawManager.Instance.UpdateGallery();
        }
    }

   public void Stage1Rule()
{
    PlayerPrefs.SetString("LastScenePlayed", "Stage 1 Rule");
    PlayerPrefs.Save();
    SceneManager.LoadScene("Stage 1 Rule");
    Time.timeScale = 1;
    SoundManager.instance.Play(SoundManager.SoundName.Click);
}

public void Stage1Play()
{
    PlayerPrefs.SetString("LastScenePlayed", "Stage 1 Play");
    PlayerPrefs.Save();
    SceneManager.LoadScene("Stage 1 Play");
    Time.timeScale = 1;
    SoundManager.instance.Play(SoundManager.SoundName.Click);
}

public void Stage2Rule()
{
    PlayerPrefs.SetString("LastScenePlayed", "Stage 2 Rule");
    PlayerPrefs.Save();
    SceneManager.LoadScene("Stage 2 Rule");
    Time.timeScale = 1;
    SoundManager.instance.Play(SoundManager.SoundName.Click);
}

public void Stage2Play()
{
    PlayerPrefs.SetString("LastScenePlayed", "Stage 2 Play");
    PlayerPrefs.Save();
    SceneManager.LoadScene("Stage 2 Play");
    Time.timeScale = 1;
    SoundManager.instance.Play(SoundManager.SoundName.Click);
}

public void Stage3Rule()
{
    PlayerPrefs.SetString("LastScenePlayed", "Stage 3 Rule");
    PlayerPrefs.Save();
    SceneManager.LoadScene("Stage 3 Rule");
    Time.timeScale = 1;
    SoundManager.instance.Play(SoundManager.SoundName.Click);
}

public void Stage3Play()
{
    PlayerPrefs.SetString("LastScenePlayed", "Stage 3 Play");
    PlayerPrefs.Save();
    SceneManager.LoadScene("Stage 3 Play");
    Time.timeScale = 1;
    SoundManager.instance.Play(SoundManager.SoundName.Click);
}

public void Stage4Rule()
{
    PlayerPrefs.SetString("LastScenePlayed", "Stage 4 Rule");
    PlayerPrefs.Save();
    SceneManager.LoadScene("Stage 4 Rule");
    Time.timeScale = 1;
    SoundManager.instance.Play(SoundManager.SoundName.Click);
}

public void Stage4Play()
{
    PlayerPrefs.SetString("LastScenePlayed", "Stage 4 Play");
    PlayerPrefs.Save();
    SceneManager.LoadScene("Stage 4 Play");
    Time.timeScale = 1;
    SoundManager.instance.Play(SoundManager.SoundName.Click);
}

public void Stage5Rule()
{
    PlayerPrefs.SetString("LastScenePlayed", "Stage 5 Rule");
    PlayerPrefs.Save();
    SceneManager.LoadScene("Stage 5 Rule");
    Time.timeScale = 1;
    SoundManager.instance.Play(SoundManager.SoundName.Click);
}

public void Stage5Play()
{
    PlayerPrefs.SetString("LastScenePlayed", "Stage 5 Play");
    PlayerPrefs.Save();
    SceneManager.LoadScene("Stage 5 Play");
    Time.timeScale = 1;
    SoundManager.instance.Play(SoundManager.SoundName.Click);
}
    
    
}
