using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    public SpriteRenderer backgroundRenderer; // Reference to the SpriteRenderer of the background
    public float scaleFactor = 1.1f; // เพิ่มตัวแปรเพื่อขยายขนาดพื้นหลัง (1.1 = ขยาย 10%)

    void Start()
    {
        AdjustBackgroundSize();
    }

    void AdjustBackgroundSize()
    {
        // Get the main camera
        Camera mainCamera = Camera.main;

        // Calculate the height of the screen in world units based on the camera
        float screenHeight = mainCamera.orthographicSize * 2;

        // Calculate the width of the screen based on the screen's aspect ratio
        float screenWidth = screenHeight * Screen.width / Screen.height;

        // Get the size of the background sprite
        float bgWidth = backgroundRenderer.sprite.bounds.size.x;
        float bgHeight = backgroundRenderer.sprite.bounds.size.y;

        // Scale the background to fit the screen, adding the scaleFactor to enlarge it
        Vector3 scale = transform.localScale;
        scale.x = (screenWidth / bgWidth) * scaleFactor; // ขยายพื้นหลังตามตัวแปร scaleFactor
        scale.y = (screenHeight / bgHeight) * scaleFactor;
        transform.localScale = scale;
    }
}