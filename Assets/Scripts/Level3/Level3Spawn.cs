using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Spawn : MonoBehaviour
{
    public List<GameObject> SpawnPositionVector;
    public List<GameObject> SpawnPosition;
    public void StartGame()
    {
        SpawnPosition = new List<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            var animal = Instantiate (Level3Global.AllItemStatic[i], SpawnPositionVector[i].transform.position, Quaternion.identity);
            animal.name = Level3Global.AllItemStatic[i].name;
            SpawnPosition.Add(animal);
        }
    }
}