using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ResponsiveUIForIsFold25G))]
public class ResponsiveUIForIsFold25GEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ResponsiveUIForIsFold25G script = (ResponsiveUIForIsFold25G)target;

        if (GUILayout.Button("Auto-Fill Default Positions"))
        {
            foreach (var settings in script.uiElements)
            {
                if (settings.element != null)
                {
                    // ตั้งค่า Default Position อัตโนมัติ
                    settings.smallDevicePosition = settings.element.anchoredPosition;
                }
            }
            Debug.Log("Default positions for ResponsiveUIForIsFold25G have been updated!");
        }

        DrawDefaultInspector(); // แสดง Inspector ปกติ
    }
}