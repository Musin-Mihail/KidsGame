using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetsController : MonoBehaviour 
{
    public List<Pets> actors = new List<Pets>();

    void Awake()
    {
        actors.AddRange(GameObject.FindObjectsOfType<Pets>());
        StartMovement();
    }

    public void StartMovement()
    {
        if (actors.Count <= 0)
        {
            Debug.Log("End");
            return;
        }

        actors[0].StartCoroutine(actors[0].StartMovement());
    }

    public Pets GetActor()
    {
        if (actors.Count > 0)
            return actors[0];

        return null;
    }
}
