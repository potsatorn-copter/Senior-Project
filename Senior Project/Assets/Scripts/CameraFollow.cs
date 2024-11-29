using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.3f;
    public Vector3 offset;
    public float cameraThreshold = 0.2f;
    public GameObject gameOverUI;

    private float highestYPosition;
    private bool gameIsOver = false;
    private bool isBouncing = false;
    private ScoremanagerScene2 scoreManager;

    private void Start()
    {
        highestYPosition = transform.position.y;
        gameOverUI.SetActive(false);

        scoreManager = FindObjectOfType<ScoremanagerScene2>();
        if (scoreManager == null)
        {
            Debug.LogWarning("ScoremanagerScene2 not found in the scene.");
        }

        InvokeRepeating("DetectAndDestroyPlatforms", 0.5f, 0.5f);
    }

    private void LateUpdate()
    {
        if (gameIsOver) return;

        if (isBouncing || target.position.y > highestYPosition + cameraThreshold)
        {
            highestYPosition = Mathf.Lerp(highestYPosition, target.position.y - cameraThreshold, 0.02f);
            Vector3 desiredPosition = new Vector3(transform.position.x, highestYPosition + offset.y, transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }

        if (target.position.y < transform.position.y - 0.7f && !gameIsOver)
        {
            isBouncing = false;
            gameIsOver = true;
            scoreManager?.EndGameWithJigsawCheck();
        }
    }

    private void DetectAndDestroyPlatforms()
    {
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("TrampolinePlatform");

        foreach (GameObject platform in platforms)
        {
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(platform.transform.position);

            if (viewportPos.y < -0.3f)
            {
                Destroy(platform);
            }
        }
    }

    public void StartBouncing()
    {
        isBouncing = true;
        StartCoroutine(StopBouncingAfterDelay(0.5f));
    }

    private IEnumerator StopBouncingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isBouncing = false;
    }
}
