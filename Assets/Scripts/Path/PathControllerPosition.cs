using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathControllerPosition : MonoBehaviour
{
    Transform[] pathTransform;

    public Vector2 offset;

    public float offsetPosition;

    void Awake()
    {
        pathTransform = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update () 
	{
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        Vector2 position = Vector2.zero;

        position.x = transform.position.x;
        position.y = max.y + offset.y;


        for (int i = 0; i < pathTransform.Length; i++)
        {
            if (pathTransform[i].transform != transform)
            {
                position.y = offset.y - offsetPosition * i;
                pathTransform[i].transform.position = position;
            }
        }
    }
}
