using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resett : MonoBehaviour
{
    public void Reset()
    {
        JigsawManager.Instance.ResetJigsawProgress();
    }
}
