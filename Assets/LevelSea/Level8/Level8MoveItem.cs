using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Level8MoveItem : MonoBehaviour
{
    public GameObject _game;
    public IEnumerator Move(int count)
    {
        Vector3 target = transform.parent.gameObject.GetComponent<Level8>().AllSpawn[count].transform.position;
        _game.GetComponent<Level8Mause>().Position = target;
        gameObject.name = transform.parent.gameObject.GetComponent<Level8>().AllPlace[count].name;
        while(transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 0.05f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}