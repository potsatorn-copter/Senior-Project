using UnityEngine;

public class UnlockImage : MonoBehaviour
{
    public Animator lockAnimator; 
    public GameObject lockedImage; 

    void Start()
    {
        lockedImage.SetActive(false);
    }

    void OnMouseDown()
    {
        StartUnlock();
    }

    private void StartUnlock()
    {
        lockAnimator.SetTrigger("Unlock");
    }

    public void ShowImage()
    {
        lockedImage.SetActive(true); 
    }
}