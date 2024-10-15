using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCard : MonoBehaviour 
{
    [SerializeField] private SceneController controller;
    [SerializeField] private GameObject Card_Back;

    private int _id;
    public int id
    {
        get { return _id; }
    }

    // เมื่อผู้เล่นคลิกการ์ด
    public void OnMouseDown()
    {
        if(Card_Back.activeSelf && controller.canReveal)
        {
            Reveal();
            controller.CardRevealed(this);
        }
    }

    // ฟังก์ชันเปลี่ยนรูปภาพของการ์ด
    public void ChangeSprite(int id, Sprite image)
    {
        _id = id;
        GetComponent<SpriteRenderer>().sprite = image; // เปลี่ยน Sprite ด้านหน้าของการ์ด
    }

    // ฟังก์ชันสำหรับแสดงการ์ด (Reveal)
    public void Reveal()
    {
        Card_Back.SetActive(false); // ซ่อนด้านหลังของการ์ด
    }

    // ฟังก์ชันสำหรับปิดการ์ด (Unreveal)
    public void Unreveal()
    {
        Card_Back.SetActive(true); // แสดงด้านหลังของการ์ด
    }
}