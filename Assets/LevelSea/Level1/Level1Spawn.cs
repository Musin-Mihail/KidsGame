using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Spawn : MonoBehaviour
{
    public List<GameObject> SpawnPositionVector;
    public List<GameObject> SpawnPosition;
    public Transform _scale;
    void Awake() 
    {
        Level1Global.Level1Spawn = gameObject;
    }
    void Start() 
    {
        SpawnPosition = new List<GameObject>();
        SpawnPosition.Add(null);
        SpawnPosition.Add(null);
        SpawnPosition.Add(null);
        SpawnItem();
    }
    public void SpawnItem()
    {
        for (int i = 0; i < SpawnPosition.Count; i++)
        {
            if(SpawnPosition[i] == null && Level1Global.AllAimalsStatic.Count > 0)
            {
                var animal = Instantiate (Level1Global.AllAimalsStatic[0], SpawnPositionVector[i].transform.position, Quaternion.identity);
                animal.transform.localScale = _scale.transform.lossyScale;
                animal.name = Level1Global.AllAimalsStatic[0].name;
                SpawnPosition[i] = animal;
                Level1Global.AllAimalsStatic.RemoveAt(0);
            }
        }
    }
}