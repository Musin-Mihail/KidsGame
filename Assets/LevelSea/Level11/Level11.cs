using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Level11 : MonoBehaviour
{
    public List<GameObject>  AllItem = new List<GameObject>();
    public List<GameObject>  AllSpawn = new List<GameObject>();
    public List<GameObject>  AllTarget = new List<GameObject>();
    public List<GameObject>  AllFishChest = new List<GameObject>();
    public static List<GameObject>  AllFishChestStatic = new List<GameObject>();
    public static List<GameObject>  AllTargetStatic = new List<GameObject>();
    public static List<GameObject>  Delete = new List<GameObject>();
    public GameObject EmptyChest;
    public GameObject FishChest;
    public GameObject TargetDistans;
    public static int count = 0;
    int HintTime;
    public static int WaitHint;
    Vector3 StartPosition;
    Vector3 EndPosition;
    public GameObject Finger;
    void Start()
    {
        AllFishChestStatic.Clear();
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
        StartCoroutine(StartHint());
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
            else
            {
                AllFishChest.Add(chest);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    public IEnumerator StartHint()
    {
        yield return new WaitForSeconds(4.0f);
        while(true)
        {
            while(HintTime < 4)
            {
                yield return new WaitForSeconds(1.0f);
                if(WaitHint == 1)
                {
                    HintTime = 0;
                    WaitHint = 0;
                    break;
                }
                HintTime++;
            }
            if(HintTime >= 4)
            {
                StartCoroutine(Hint());
            }
            HintTime = 0;
            yield return new WaitForSeconds(1.0f);
        }
    }
    public IEnumerator Hint()
    {
        if(WinBobbles.Victory > 0)
        {

            List<GameObject> newlist = AllFishChest.Where(x => x.name == "FishChest").OrderBy(x => Vector3.Distance(Finger.transform.position, x.transform.position)).ToList();
            EndPosition = newlist[0].transform.position;
            while(Finger.transform.position != EndPosition)
            {
                Finger.transform.position = Vector3.MoveTowards(Finger.transform.position, EndPosition, 0.1f);
                if(WaitHint == 1)
                {
                    Finger.transform.position = new Vector3 (0,-6,0);
                    break;
                }
                yield return new WaitForSeconds(0.01f);
            }
            Finger.transform.position = new Vector3 (0,-6,0);
        }
    }
}