using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level7Global : MonoBehaviour
{
    public List<GameObject>  ThreeFigures = new List<GameObject>(); // Хранятся список всех животных на уровне
    public List<GameObject>  AllItem = new List<GameObject>(); // Хранятся список игровых фигур
    public static List<GameObject>  AllItemStatic = new List<GameObject>(); // Хранятся список статических  игровых фигур
    public List<GameObject>  AllEmpty = new List<GameObject>(); // Хранятся список игровых фигур
    public static List<GameObject>  AllCollected = new List<GameObject>(); // Хранятся список угаданных игровых фигру
    public GameObject Finger;
    public static int WaitHint = 0;
    Vector3 Center; // Середина
    Vector3 EndTarget; // Точка после набора нужных фигур
    public int StageMove = 0;
    int HintTime = 0;
    int Stop = 0;
    static public int ThreeFiguresComplete;
    public GameObject Task;
    public GameObject Figure;//Задание от животного в центре
    public GameObject Animal;//Текущее животное в центре.
    static public int NextFigure = 0;
    Vector3 StartPosition;
    Vector3 EndPosition;
    
    void Awake()
    {
        ThreeFiguresComplete = -1;
        Center = new Vector3(0,0,3);
        EndTarget = new Vector3(15,0,3);
//Перемешивания списка.
        WinBobbles.Victory = 1;
        AllItemStatic = AllItem;
        AllCollected = new List<GameObject>();
    }
    void Start() 
    {
        EndPosition = GetComponent<Level7Spawn>().TargetPosition[4].transform.position;
        StartCoroutine(StartHint());
        StartCoroutine(ChangeTasks());
    }
    void Update()
    {
        if (WinBobbles.Victory == 0 && Stop == 0)
        {
            Stop = 1;
            Debug.Log("Победа");
            // StartCoroutine(Win2());
        }
        // if(NextFigure == 1)
        // {
        //     NextFigure = 0;
        //     FigureChange();
        // }
    }
    IEnumerator ChangeTasks() 
    {
        for (int i = 0; i < 5; i++)
        {
            RandomItem();
            while(NextFigure != 1)
            {
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(2.0f);
            NextFigure = 0;
        }
        GetComponent<Level7Spawn>().DestroyAll();
        WinBobbles.Victory = 0;
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
        GetComponent<Level7Spawn>().StartGame();
    }
    void FigureChange()
    {
        if(ThreeFiguresComplete >= 0)
        {
            ThreeFigures[ThreeFiguresComplete].GetComponent<SpriteRenderer>().enabled = true;
            ThreeFigures[ThreeFiguresComplete].GetComponent<SpriteRenderer>().sprite = Figure.GetComponent<SpriteRenderer>().sprite;
        }
        if(GetComponent<Level3Spawn>().SpawnPosition.Count > 2)
        {
            int NewRandom = Random.Range(0, GetComponent<Level3Spawn>().SpawnPosition.Count);
            if(GetComponent<Level3Spawn>().SpawnPosition[NewRandom] == null)
            {
                FigureChange();
            }
            else
            {
                Figure.GetComponent<SpriteRenderer>().sprite = GetComponent<Level3Spawn>().SpawnPosition[NewRandom].GetComponent<SpriteRenderer>().sprite;
                Animal.name = GetComponent<Level3Spawn>().SpawnPosition[NewRandom].name;
                StartPosition = GetComponent<Level3Spawn>().SpawnPosition[NewRandom].transform.position;
                GetComponent<Level3Spawn>().SpawnPosition.RemoveAt(NewRandom);
            }
        }
        else
        {
            StageMove = 1;
        }
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
            var _Name = GetComponent<Level7Spawn>().TargetPosition[4].name;
            foreach (var item in GetComponent<Level7Spawn>().SpawnPosition)
            {
                if (item.name == _Name)
                {
                    StartPosition = item.transform.position;
                    break;
                } 
            }
            
            StartPosition.z = -1;
            EndPosition.z = -1;
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