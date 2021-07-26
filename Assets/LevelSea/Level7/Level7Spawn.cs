using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level7Spawn : MonoBehaviour
{
    public List<GameObject> SpawnPositionVector = new List<GameObject>(); // Места спавна
    public List<GameObject> SpawnPosition = new List<GameObject>(); // Появившиеся предметы на сцене
    public List<GameObject> TargetPosition = new List<GameObject>(); // Места на доске с заданием
    Sprite Circle;
    Vector3 StartScale;
    void Start()
    {
        Circle = TargetPosition[4].GetComponent<SpriteRenderer>().sprite;
        StartScale = TargetPosition[4].transform.localScale;
    }
    public void StartGame()
    {
        TargetPosition[4].GetComponent<SpriteRenderer>().sprite = Circle;
        TargetPosition[4].transform.localScale = StartScale;
        for (int i = 0; i < SpawnPosition.Count; i++)
        {
            if (SpawnPosition[i] != null)
            {
                Destroy(SpawnPosition[i].gameObject);
            }
            var animal = Instantiate (Level7Global.AllItemStatic[i], SpawnPositionVector[i].transform.position, Quaternion.identity);
            animal.name = Level7Global.AllItemStatic[i].name;
            SpawnPosition[i] = animal;
        }
        for (int i = 0; i < SpawnPosition.Count; i++)
        {
            int chance = Random.Range(0,SpawnPosition.Count-1);
            var item = SpawnPosition[i];
            SpawnPosition[i] = SpawnPosition[chance];
            SpawnPosition[chance] = item;
        }
        TargetPosition[0].GetComponent<SpriteRenderer>().sprite = SpawnPosition[1].GetComponent<SpriteRenderer>().sprite;
        TargetPosition[0].transform.localScale = SpawnPosition[1].transform.localScale*100;
        TargetPosition[1].GetComponent<SpriteRenderer>().sprite = SpawnPosition[2].GetComponent<SpriteRenderer>().sprite;
        TargetPosition[1].transform.localScale = SpawnPosition[2].transform.localScale*100;
        TargetPosition[2].GetComponent<SpriteRenderer>().sprite = SpawnPosition[1].GetComponent<SpriteRenderer>().sprite;
        TargetPosition[2].transform.localScale = SpawnPosition[1].transform.localScale*100;
        TargetPosition[3].GetComponent<SpriteRenderer>().sprite = SpawnPosition[2].GetComponent<SpriteRenderer>().sprite;
        TargetPosition[3].transform.localScale = SpawnPosition[2].transform.localScale*100;
        TargetPosition[4].name = SpawnPosition[1].name;
        Level7Global.WaitHint = 1;
    }
    public void DestroyAll()
    {
        for (int i = 0; i < SpawnPosition.Count; i++)
        {
            SpawnPosition[i].gameObject.SetActive(false);
            // Destroy(SpawnPosition[i].gameObject);
        }
    }
}