using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Spawn : MonoBehaviour
{
    public List<GameObject> SpawnPositionVector = new List<GameObject>();
    public List<GameObject> SpawnPosition = new List<GameObject>();
    void Update()
    {
        for (int i = 0; i < SpawnPosition.Count; i++)
        {
            if(SpawnPosition[i] == null && Level2Global.AllItemStatic.Count > 0)
            {
                var animal = Instantiate (Level2Global.AllItemStatic[0], SpawnPositionVector[i].transform.position, Quaternion.identity);
                animal.name = Level2Global.AllItemStatic[0].name;
                SpawnPosition[i] = animal;
                Level2Global.AllItemStatic.RemoveAt(0);
            }
        }
    }
}