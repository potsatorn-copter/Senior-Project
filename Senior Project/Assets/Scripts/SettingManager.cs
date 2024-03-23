using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
   [SerializeField] private GameObject[] SoundDisplay;  // ประกาศอาร์เรย์ของ GameObjects ที่คุณต้องการแสดง
   [SerializeField] private GameObject StartMenu;
    
        private bool areButtonsVisible = false;
        
        public void ToggleButtons()
        {
            areButtonsVisible = !areButtonsVisible; // เปลี่ยนสถานะของการแสดงปุ่ม
            
            // วนลูปเรียกเมท็อด SetActive() เพื่อแสดงหรือซ่อนปุ่ม
            foreach (GameObject button in SoundDisplay)
            {
                button.SetActive(areButtonsVisible);
            }

            if (areButtonsVisible == true)
            {
                StartMenu.SetActive(false);
            }
            else
            {
                StartMenu.SetActive(true);
            }
            
        }
}
