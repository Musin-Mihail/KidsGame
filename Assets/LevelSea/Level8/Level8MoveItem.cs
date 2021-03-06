using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Level8MoveItem : MonoBehaviour
{
    // void Start()
    // {
    //     StartCoroutine(Move());
    // }
    public IEnumerator Move()
    {
        int number = Int32.Parse(gameObject.name);
        Debug.Log("1");
        Vector3 target = Level8Spawn.SpawnPosition3Static[number].transform.position;
        Debug.Log("2");
        GetComponent<Level8MauseClick>().Position = Level8Spawn.SpawnPosition3Static[number].transform.position;
        Debug.Log("3");
        while(transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 0.05f);
            yield return new WaitForSeconds(0.01f);
        }
        Debug.Log("End");
    }
}
