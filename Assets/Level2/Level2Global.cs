using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Global : MonoBehaviour
{
    public List<GameObject>  AllItem = new List<GameObject>(); // Хранятся список игровых фигур
    public static List<GameObject>  AllItemStatic = new List<GameObject>(); // Хранятся список статических  игровых фигур
    public List<GameObject>  AllEmpty = new List<GameObject>(); // Хранятся список игровых фигур
    public static List<GameObject>  AllCollected = new List<GameObject>(); // Хранятся список угаданных игровых фигру
    public GameObject Finger;
    public GameObject Boat;
    public Vector3 TargetBoat;
    public static int WaitHint = 0;
    int HintTime =0;
    int Stop = 0;
    
    void Awake()
    {
//Перемешивания списка.
        WinBobbles.Victory = AllItem.Count;
        for (int i = 0; i < AllItem.Count; i++)
        {
            int chance = Random.Range(0,AllItem.Count-1);
            var item = AllItem[i];
            AllItem[i] = AllItem[chance];
            AllItem[chance] = item;
        }
        AllItemStatic = AllItem;
        AllCollected = new List<GameObject>();
        TargetBoat = new Vector3(-15, 1.1f, 2.89f);
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
            Debug.Log("Победа");
            StartCoroutine(Win2());
        }
    }
    IEnumerator Win2()
    {
        while(Boat.transform.position != TargetBoat)
        {
            Boat.transform.position = Vector3.MoveTowards(Boat.transform.position, TargetBoat, 0.1f);
            yield return new WaitForSeconds(0.02f);
        }
    }
    public IEnumerator StartHint()
    {
        while(true)
        {
            while(HintTime < 5)
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
            if(HintTime >= 5)
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

        foreach (var item in GetComponent<Level2Spawn>().SpawnPosition)
        {
            // if(item!= null && item.transform.position == item.GetComponent<MoveItem>().StartPosition)
            if(item != null)
            {
                Tag = item.name;
                Start = item.transform.position;
                Start.z += -1;
                check = 1;
                break;
            }
        }
        if(check == 1)
        {
            foreach (var item in AllEmpty)
            {
                if(Tag == item.name)
                {
                    check = 2;
                    End = item.transform.position;
                    End.z += -1;
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
                yield return new WaitForSeconds(0.02f);
            }
        }
        Finger.transform.position = new Vector3 (0,10,0);
    }
}