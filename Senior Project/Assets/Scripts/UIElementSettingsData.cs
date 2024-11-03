using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIElementSettingsData", menuName = "UI Settings Data")]
public class UIElementSettingsData : ScriptableObject
{
    [System.Serializable]
    public class ElementData
    {
        public Vector2 position;
        public Vector3 scale;
    }

    public List<ElementData> elementDataList = new List<ElementData>();
}