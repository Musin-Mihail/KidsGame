using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4Spawn : MonoBehaviour
{
    public List<GameObject> SpawnPositionVector;
    public List<GameObject> SpawnPosition;
    public Transform _scale;
    void Awake() 
    {
        Level4Global._level4Spawn = gameObject;
    }
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            SpawnAnimal(i);
        }
    }
    public void SpawnAnimal(int number)
    {
        if(Level4Global.AllAimalsStatic.Count > 0)
        {
            var animal = Instantiate (Level4Global.AllAimalsStatic[0], SpawnPositionVector[number].transform.position, Quaternion.identity);
            animal.transform.localScale = _scale.lossyScale;
            animal.name = Level4Global.AllAimalsStatic[0].name;
            SpawnPosition[number] = animal;
            Level4Global.AllAimalsStatic.RemoveAt(0);
        }
    }
    public void SearchFreeSpace(string nameAnimal)
    {
        for (int i = 0; i < 3; i++)
        {
            if(nameAnimal == SpawnPosition[i].name)
            {
                SpawnAnimal(i);
            }
        }
    }
}