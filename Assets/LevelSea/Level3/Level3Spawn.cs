using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Spawn : MonoBehaviour
{
    public List<GameObject> SpawnPositionVector = new List<GameObject>();
    public List<GameObject> SpawnPosition = new List<GameObject>();
    public void StartGame()
    {
        for (int i = 0; i < SpawnPosition.Count; i++)
        {
            var animal = Instantiate (Level3Global.AllItemStatic[i], SpawnPositionVector[i].transform.position, Quaternion.identity);
            animal.name = Level3Global.AllItemStatic[i].name;
            SpawnPosition[i] = animal;
        }
    }
}