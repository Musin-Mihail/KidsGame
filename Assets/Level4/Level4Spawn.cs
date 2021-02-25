using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4Spawn : MonoBehaviour
{
    public List<GameObject> SpawnPositionVector = new List<GameObject>();
    public List<GameObject> SpawnPosition = new List<GameObject>();
    void Update()
    {
        for (int i = 0; i < SpawnPosition.Count; i++)
        {
            if(SpawnPosition[i] == null && Level4Global.AllAimalsStatic.Count > 0)
            {
                var animal = Instantiate (Level4Global.AllAimalsStatic[0], SpawnPositionVector[i].transform.position, Quaternion.identity);
                animal.name = Level4Global.AllAimalsStatic[0].name;
                SpawnPosition[i] = animal;
                Level4Global.AllAimalsStatic.RemoveAt(0);
            }
        }
    }
}