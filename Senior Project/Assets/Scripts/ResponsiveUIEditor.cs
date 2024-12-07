using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

[CustomEditor(typeof(ResponsiveUI))]
public class ResponsiveUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ResponsiveUI script = (ResponsiveUI)target;

        if (GUILayout.Button("Auto-Fill Default Positions"))
        {
            foreach (var settings in script.uiElements)
            {
                if (settings.element != null)
                {
                    // ตั้งค่า Default Position อัตโนมัติ
                    settings.iPadPosition = settings.element.anchoredPosition;
                }
            }
            Debug.Log("Default positions for ResponsiveUI have been updated!");
        }

        DrawDefaultInspector(); // แสดง Inspector ปกติ
    }
}