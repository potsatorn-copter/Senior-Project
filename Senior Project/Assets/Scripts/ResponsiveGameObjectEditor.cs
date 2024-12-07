using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ResponsiveGameObject))]
public class ResponsiveGameObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // ดึงข้อมูล Target Script
        ResponsiveGameObject script = (ResponsiveGameObject)target;

        // ปุ่มอัตโนมัติสำหรับดึงค่า Default Position
        if (GUILayout.Button("Auto-Fill Default Positions"))
        {
            foreach (var settings in script.gameObjectSettings)
            {
                if (settings.target != null)
                {
                    // ตั้งค่า Default Position ตามตำแหน่งปัจจุบันของ GameObject
                    settings.defaultPosition = settings.target.localPosition;
                }
            }
            Debug.Log("Default Positions Updated!");
        }

        // แสดง Inspector ปกติ
        DrawDefaultInspector();
    }
}