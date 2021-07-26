using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4Global : MonoBehaviour
{
    public List<GameObject>  AllAnimals = new List<GameObject>();
    public static List<GameObject>  AllCollected = new List<GameObject>();
    public static List<GameObject>  AllAimalsStatic = new List<GameObject>();
    public List<GameObject>  AllZone = new List<GameObject>();
    public GameObject Finger;
    public static int WaitHint = 0;
    int HintTime = 0;
    int Stop = 0;
    public static GameObject _level4Spawn; 
    void Awake()
    {
        WinBobbles.Victory = AllAnimals.Count;
        for (int i = 0; i < AllAnimals.Count; i++)
        {
            int chance = Random.Range(0,9);
            var item = AllAnimals[i];
            AllAnimals[i] = AllAnimals[chance];
            AllAnimals[chance] = item;
        }
        AllAimalsStatic = AllAnimals;
        AllCollected = new List<GameObject>();
    }
    void Start() 
    {
        StartCoroutine(StartHint());
    }
    void Update()
    {
        if (WinBobbles.Victory == 0 && Stop == 0)
        {
            Stop = 1;
            StartCoroutine(Win2());
        }
    }
    IEnumerator Win2()
    {
        foreach (var item in AllCollected)
        {
            StartCoroutine(item.GetComponent<WinUp>().Win());
            yield return new WaitForSeconds(0.05f);
        }
    }
    public IEnumerator StartHint()
    {
        while(WinBobbles.Victory != 0)
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
        Vector3 Start = new Vector3(0,10,0);
        Vector3 End = new Vector3(0,10,0);
        int check = 0;
        string Tag = "";

        foreach (var item in GetComponent<Level4Spawn>().SpawnPosition)
        {
            if(item != null)
            {
                Tag = item.tag;
                Start = item.transform.position;
                check = 1;
                break;
            }
        }
        if(check == 1)
        {
            foreach (var item in AllZone)
            {
                if(Tag == item.tag)
                {
                    check = 2;
                    End = item.transform.position;
                    break;
                }
            }
        }
        Start.z = -1;
        Finger.transform.position = Start;
        if(check == 2)
        {
            while(Finger.transform.position != End)
            {
                Finger.transform.position = Vector3.MoveTowards(Finger.transform.position,End, 0.1f);
                if(WaitHint == 1)
                {
                    Finger.transform.position = new Vector3 (0,10,0);
                    break;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
        Finger.transform.position = new Vector3 (0,10,0);
    }
}