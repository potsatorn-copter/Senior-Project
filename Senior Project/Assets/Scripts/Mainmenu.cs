using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Stage");
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }

    public void QuitGame()
    {
        Application.Quit();
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }

    public void Back()
    {
        SceneManager.LoadScene("Mainmenu");
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }

    public void Stage1Rule()
    {
        SceneManager.LoadScene("Stage 1 Rule");
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }
    public void Stage2Rule()
    {
        SceneManager.LoadScene("Stage 2 Rule");
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }

    public void Stage1()
    {
        SceneManager.LoadScene("Stage 1");
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }
    public void Stage2()
    {
        SceneManager.LoadScene("Stage 2");
        SoundManager.instance.Play(SoundManager.SoundName.Click);
    }
}
