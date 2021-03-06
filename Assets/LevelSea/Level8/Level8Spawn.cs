using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level8Spawn : MonoBehaviour
{
    public static int Count = 4;
    public List<GameObject> AllPuzzle = new List<GameObject>(); // Места спавна
    public static List<GameObject> AllPuzzleStatic = new List<GameObject>();
    public List<GameObject> SpawnPositionVector = new List<GameObject>(); // Места спавна
    public List<GameObject> SpawnPosition1 = new List<GameObject>(); // Появившиеся предметы на сцене
    public static List<GameObject> SpawnPosition1Static = new List<GameObject>(); // Появившиеся предметы на сцене
    public List<GameObject> SpawnPosition2 = new List<GameObject>(); // Появившиеся предметы на сцене
    public static List<GameObject> SpawnPosition2Static = new List<GameObject>(); // Появившиеся предметы на сцене
    public List<GameObject> SpawnPosition3 = new List<GameObject>(); // Появившиеся предметы на сцене
    public static List<GameObject> SpawnPosition3Static = new List<GameObject>(); // Появившиеся предметы на сцене
    public List<GameObject>  Puzzle1 = new List<GameObject>();
    public static List<GameObject>  Puzzle1Static = new List<GameObject>();
    private void Awake() 
    {
        // for (int i = 0; i < SpawnPosition1.Count; i++)
        // {
        //     int chance = Random.Range(0,SpawnPosition1.Count-1);
        //     var item = SpawnPosition1[i];
        //     SpawnPosition1[i] = SpawnPosition1[chance];
        //     SpawnPosition1[chance] = item;
        // }
        // for (int i = 0; i < SpawnPosition1.Count; i++)
        // {
        //     int chance = Random.Range(0,SpawnPosition2.Count-1);
        //     var item = SpawnPosition2[i];
        //     SpawnPosition2[i] = SpawnPosition2[chance];
        //     SpawnPosition2[chance] = item;
        // }
        // for (int i = 0; i < SpawnPosition3.Count; i++)
        // {
        //     int chance = Random.Range(0,SpawnPosition3.Count-1);
        //     var item = SpawnPosition3[i];
        //     SpawnPosition3[i] = SpawnPosition3[chance];
        //     SpawnPosition3[chance] = item;
        // }
        Puzzle1Static = Puzzle1;
        SpawnPosition1Static = SpawnPosition1;
        SpawnPosition2Static = SpawnPosition3;
        SpawnPosition3Static = SpawnPosition3;
        AllPuzzleStatic = AllPuzzle;
    }
}