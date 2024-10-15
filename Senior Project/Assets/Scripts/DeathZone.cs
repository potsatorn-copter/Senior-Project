using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private GameObject endgGameObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // เมื่อผู้เล่นชนกับ DeathZone ให้เรียกฟังก์ชัน EndGame
            ScoremanagerScene2.Instance.EndGame(); // ทำให้เกมจบทันที
            endgGameObject.SetActive(true);
            Debug.Log("Player fell into the death zone. Game Over.");
        }
    }
}
