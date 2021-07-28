using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAnimation : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<Animator>().enabled = false;
        transform.localScale = Level11._scaleStatic.lossyScale;
    }
    // public void DisableAnimation2()
    // {
        
    // }
}
