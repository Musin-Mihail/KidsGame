using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation360 : MonoBehaviour
{
    IEnumerator Start()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.02f);
            transform.Rotate(0.0f, 0.0f, -0.1f, Space.World);
        }
    }
}