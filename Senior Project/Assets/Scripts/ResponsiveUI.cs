using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class UIElementSettings
{
    public RectTransform element; // UI Element
    public Vector2 iPadPosition; // ตำแหน่งเมื่ออยู่ใน iPad
    public Vector3 iPadScale = Vector3.one; // ขนาดสเกลเมื่ออยู่ใน iPad
    public bool scaleOnIpad = false; // กำหนดว่าควรจะขยายใน iPad หรือไม่
}

public class ResponsiveUI : MonoBehaviour
{
    public List<UIElementSettings> uiElements; // รายการ UI Elements พร้อมการตั้งค่าสำหรับแต่ละอัน

    [SerializeField] private RectTransform canvasRect;
    private List<Vector2> defaultPositions = new List<Vector2>(); // เก็บตำแหน่งเริ่มต้นของ UI
    private List<Vector3> defaultScales = new List<Vector3>(); // เก็บขนาดเริ่มต้นของ UI

    private void Start()
    {
        Canvas parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas != null)
        {
            canvasRect = parentCanvas.GetComponent<RectTransform>(); // รับ RectTransform ของ Canvas
        }

        if (canvasRect == null)
        {
            Debug.LogError("ไม่พบ RectTransform ของ Canvas!");
            return;
        }

        // บันทึกตำแหน่งและสเกลเริ่มต้นของ UI Elements
        foreach (var settings in uiElements)
        {
            if (settings.element != null)
            {
                defaultPositions.Add(settings.element.anchoredPosition);
                defaultScales.Add(settings.element.localScale);
            }
            else
            {
                Debug.LogWarning("พบ UI Element ที่ไม่ได้ตั้งค่าไว้ใน List.");
                defaultPositions.Add(Vector2.zero);
                defaultScales.Add(Vector3.one);
            }
        }

        AdjustUIPositions();
    }

    private void Update()
    {
        AdjustUIPositions(); // อัพเดตตำแหน่ง UI ระหว่างการทดสอบใน Editor
    }

    private void AdjustUIPositions()
    {
        if (uiElements.Count != defaultPositions.Count || uiElements.Count != defaultScales.Count)
        {
            Debug.LogError("จำนวน UI Elements, defaultPositions และ defaultScales ไม่ตรงกัน");
            return;
        }

        bool isIpad = (canvasRect != null && canvasRect.rect.width >= 1900 && canvasRect.rect.height >= 1300);

        for (int i = 0; i < uiElements.Count; i++)
        {
            if (uiElements[i].element != null)
            {
                uiElements[i].element.anchoredPosition = isIpad ? uiElements[i].iPadPosition : defaultPositions[i];
                
                if (isIpad && uiElements[i].scaleOnIpad)
                {
                    uiElements[i].element.localScale = uiElements[i].iPadScale;
                }
                else
                {
                    uiElements[i].element.localScale = defaultScales[i];
                }
            }
        }
    }
}