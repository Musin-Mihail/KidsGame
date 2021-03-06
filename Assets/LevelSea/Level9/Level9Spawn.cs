using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level9Spawn : MonoBehaviour
{
    public List<GameObject> SpawnPositionVector = new List<GameObject>(); // Места спавна
    public List<GameObject> SpawnPosition = new List<GameObject>(); // Появившиеся предметы на сцене
    public static int Next = 1;
    private void Update() 
    {
        if(Next == 1)
        {
            Next = 0;
            // StartGame();
            Invoke("StartGame", 0.1f);
        }
    }
    public void StartGame()
    {
        for (int i = 0; i < SpawnPosition.Count; i++)
        {
            if(Level9Global.AllItemStatic.Count > 0 && SpawnPosition[i] == null)
            {
                var animal = Instantiate (Level9Global.AllItemStatic[0], SpawnPositionVector[i].transform.position, Quaternion.identity);
                // animal.name = Level9Global.AllItemStatic[i].name;
                SpawnPosition[i] = animal;
                Level9Global.AllItemStatic.RemoveAt(0);       
            } 
        }
    }
    public void DestroyAll()
    {
        for (int i = 0; i < SpawnPosition.Count; i++)
        {
            Destroy(SpawnPosition[i].gameObject);
        }
    }
}