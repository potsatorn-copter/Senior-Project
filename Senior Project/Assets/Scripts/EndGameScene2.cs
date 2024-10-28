using System.Collections;
using UnityEngine;

public class EndGameScene2 : MonoBehaviour
{
    public GameObject uiToShow;
    public string playerTag = "Player";
    private ScoremanagerScene2 scoreManager;

    private void Start()
    {
        if (uiToShow != null)
        {
            uiToShow.SetActive(false);
        }

        scoreManager = FindObjectOfType<ScoremanagerScene2>();
        if (scoreManager == null)
        {
            Debug.LogWarning("ScoremanagerScene2 not found in the scene.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Player hit the item!");

            if (scoreManager != null)
            {
                scoreManager.EndGame();

                // แสดง EndGamePanel ทันทีโดยไม่ต้องรอ JigsawUIPanel
                ShowEndGamePanel();
            }
        }
    }

    private void ShowEndGamePanel()
    {
        if (uiToShow != null)
        {
            Debug.Log("EndGamePanel is now visible.");
            uiToShow.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            Debug.LogWarning("EndGamePanel (uiToShow) is not assigned in the Inspector.");
        }
    }
    
}