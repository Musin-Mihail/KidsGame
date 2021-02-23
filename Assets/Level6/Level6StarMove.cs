using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level6StarMove : MonoBehaviour
{
    public int State = 1;
    public Vector3 StartPosition;
    Vector3 SpawnPosition;
    void Start()
    {
        StartPosition = transform.position;
        SpawnPosition = transform.position;
        SpawnPosition.y += 5;
        transform.position = SpawnPosition;
        StartCoroutine(Rotation());
        StartCoroutine(StartMove());
    }

    void Update()
    {
        
    }
    IEnumerator StartMove()
    {
        while(transform.position != StartPosition && State == 1)
        {
            transform.position = Vector2.MoveTowards(transform.position,StartPosition, 0.1f);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.05f);
        StartCoroutine(StartMove());
    }
    IEnumerator Rotation()
    {
        while(State == 1)
        {
            transform.rotation *= Quaternion.Euler(0, 0, 5);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.05f);
        StartCoroutine(Rotation());
    }
}