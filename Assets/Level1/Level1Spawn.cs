using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Spawn : MonoBehaviour
{
    public List<GameObject> SpawnPositionVector = new List<GameObject>();
    public List<GameObject> SpawnPosition = new List<GameObject>();
    void Update()
    {
        for (int i = 0; i < SpawnPosition.Count; i++)
        {
            if(SpawnPosition[i] == null && Level1Global.AllAimalsStatic.Count > 0)
            {
                var animal = Instantiate (Level1Global.AllAimalsStatic[0], SpawnPositionVector[i].transform.position, Quaternion.identity);
                animal.name = Level1Global.AllAimalsStatic[0].name;
                SpawnPosition[i] = animal;
                Level1Global.AllAimalsStatic.RemoveAt(0);
            }
        }
    }
}