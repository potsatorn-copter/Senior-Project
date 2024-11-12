using System.Collections;
using UnityEngine;

public class EndGameScene2 : MonoBehaviour
{
    public GameObject uiToShow; // UI ที่จะโชว์เมื่อชนกับไอเท็ม
    public string playerTag = "Player"; // Tag ของ Player เพื่อเช็คการชน
    public float delayBeforeShowingUI = 0.5f; // เวลาที่จะหน่วงก่อนโชว์ UI
    private ScoremanagerScene2 scoreManager; // ตัวแปรเก็บอ้างอิงถึง ScoremanagerScene2

    private void Start()
    {
        if (uiToShow != null)
        {
            uiToShow.SetActive(false); // ซ่อน UI ตอนเริ่มต้น
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
            ShowUI();
            //if (scoreManager != null) scoreManager.EndGame();
        }
    }

    private void ShowUI()
    {
        if (uiToShow != null)
        {
            Debug.Log("Showing UI");
            uiToShow.SetActive(true);
        }

        Time.timeScale = 0f;
    }
}