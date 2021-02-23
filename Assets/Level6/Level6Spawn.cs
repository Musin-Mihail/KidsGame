using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level6Spawn : MonoBehaviour
{
    public List<GameObject> SpawnPositionVector = new List<GameObject>();
    public List<GameObject> SpawnPosition = new List<GameObject>();
    void Update()
    {
        for (int i = 0; i < SpawnPosition.Count; i++)
        {
            if(SpawnPosition[i] == null && Level6Global.AllStarsStatic.Count > 0)
            {
                var star = Instantiate (Level6Global.AllStarsStatic[0], SpawnPositionVector[i].transform.position, Quaternion.identity);
                SpawnPosition[i] = star;
                Level6Global.AllStarsStatic.RemoveAt(0);
            }
        }
    }
}