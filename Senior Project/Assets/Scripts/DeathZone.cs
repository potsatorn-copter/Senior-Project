using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private GameObject endgGameObject;
    private ScoremanagerScene2 scoreManager; // ตัวแปรเก็บอ้างอิงถึง ScoremanagerScene2

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoremanagerScene2>();
        if (scoreManager == null)
        {
            Debug.LogWarning("ScoremanagerScene2 not found in the scene.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (scoreManager != null) scoreManager.EndGameWithJigsawCheck();
            endgGameObject.SetActive(true);
            Debug.Log("Player fell into the death zone. Game Over.");
        }
    }
}