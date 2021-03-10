using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level9Global : MonoBehaviour
{
    public List<GameObject>  AllItem = new List<GameObject>(); // Хранятся список 
    public static List<GameObject>  AllItemStatic = new List<GameObject>(); // Хранятся список статических  игровых фигур
    public List<GameObject>  AllEmpty = new List<GameObject>(); // Хранятся список игровых фигур
    public static List<GameObject>  AllCollected = new List<GameObject>(); // Хранятся список угаданных игровых фигру
    public GameObject Finger;
    public static int touch = 0;
    public static int WaitHint = 0;
    Vector3 Center; // Середина
    Vector3 EndTarget; // Точка после набора нужных фигур
    // public int StageMove = 0;
    int HintTime = 0;
    int Stop = 0;
    // static public int ThreeFiguresComplete;
    // static public int NextFigure = 0;
    Vector3 StartPosition;
    Vector3 EndPosition;
    
    void Awake()
    {
        touch = 0;
        for (int i = 0; i < AllItem.Count; i++)
        {
            int chance = Random.Range(0,AllItem.Count);
            var item = AllItem[i];
            AllItem[i] = AllItem[chance];
            AllItem[chance] = item;
        }
        AllItemStatic = AllItem;
        // ThreeFiguresComplete = -1;
        Center = new Vector3(0,0,3);
        EndTarget = new Vector3(15,0,3);
        WinBobbles.Victory = AllItem.Count;
        AllCollected = new List<GameObject>();
    }
    void Start() 
    {
        StartCoroutine(StartHint());
        // StartCoroutine(ChangeTasks());
    }
    void Update()
    {
        if (WinBobbles.Victory == 0 && Stop == 0)
        {
            Stop = 1;
            Debug.Log("Победа");
            // StartCoroutine(Win2());
        }
    }
    void RandomItem()
    {
        for (int i = 0; i < AllItem.Count; i++)
        {
            int chance = Random.Range(0,AllItem.Count-1);
            var item = AllItem[i];
            AllItem[i] = AllItem[chance];
            AllItem[chance] = item;
        }
        GetComponent<Level9Spawn>().StartGame();
    }

    // IEnumerator Win2()
    // {
    //     foreach (var item in AllCollected)
    //     {
    //         StartCoroutine(item.GetComponent<WinUp>().Win());
    //         yield return new WaitForSeconds(0.05f);
    //     }
    // }
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
        if(WinBobbles.Victory > 0)
        {
            StartPosition = GetComponent<Level9Spawn>().SpawnPosition[0].transform.position;
            string _tag = GetComponent<Level9Spawn>().SpawnPosition[0].tag;
            foreach (var item in AllEmpty)
            {
                if(item.tag == _tag)
                {
                    EndPosition = item.transform.position;
                    break;
                }
            }
            Finger.transform.position = StartPosition;
            while(Finger.transform.position != EndPosition)
            {
                Finger.transform.position = Vector3.MoveTowards(Finger.transform.position, EndPosition, 0.1f);
                if(WaitHint == 1)
                {
                    Finger.transform.position = new Vector3 (0,10,0);
                    break;
                }
                yield return new WaitForSeconds(0.02f);
            }
            Finger.transform.position = new Vector3 (0,10,0);
        }
    }
}