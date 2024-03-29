using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveItem : MonoBehaviour
{
    public int State = 1;
    public Vector3 StartPosition;
    Vector3 SpawnPosition;
    void Start()
    {
        StartPosition = transform.position;
        SpawnPosition = transform.position + Direction.directionStatic;
        transform.position = SpawnPosition;
        if (gameObject.layer == LayerMask.NameToLayer("Star"))
        {
            StartCoroutine(Rotation());
        }
        StartCoroutine(StartMove());
    }
    IEnumerator StartMove()
    {
        while(transform.position != StartPosition && State == 1)
        {
            transform.position = Vector2.MoveTowards(transform.position,StartPosition, 0.02f);
            yield return new WaitForSeconds(0.01f);
        }
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