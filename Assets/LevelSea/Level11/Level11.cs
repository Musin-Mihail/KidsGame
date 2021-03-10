using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Level11 : MonoBehaviour
{
    public List<GameObject>  AllItem = new List<GameObject>();
    public List<GameObject>  AllSpawn = new List<GameObject>();
    public List<GameObject>  AllTarget = new List<GameObject>();
    public static List<GameObject>  AllTargetStatic = new List<GameObject>();
    public static List<GameObject>  Delete = new List<GameObject>();
    public GameObject EmptyChest;
    public GameObject FishChest;
    public GameObject TargetDistans;
    public static int count = 0;
    void Start()
    {
        count = 0;
        AllTargetStatic = AllTarget;
        WinBobbles.Victory = 8;
        for (int i = 0; i < 8; i++)
        {
            AllItem.Add(FishChest);
        }
        for (int i = 0; i < 24; i++)
        {
            AllItem.Add(EmptyChest);
        }
        for (int i = 0; i < AllItem.Count; i++)
        {
            int chance = Random.Range(0,AllItem.Count-1);
            var item = AllItem[i];
            AllItem[i] = AllItem[chance];
            AllItem[chance] = item;
        }
        AllSpawn = AllSpawn.OrderBy(x => Vector2.Distance(TargetDistans.transform.position,x.transform.position)).ToList();
        StartCoroutine(StartGame());
    }
    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < AllSpawn.Count; i++)
        {
            var chest = Instantiate (AllItem[i],AllSpawn[i].transform.position, Quaternion.identity);
            chest.name = AllItem[i].name;
            if(chest.name == "EmptyChest")
            {
                Delete.Add(chest);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
