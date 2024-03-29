using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Global : MonoBehaviour
{
    public List<GameObject>  AllAnimals = new List<GameObject>(); // Хранятся список игровых фигру
    public List<GameObject>  AllEmpty = new List<GameObject>(); // Хранятся список игровых фигру
    public static List<GameObject>  AllAimalsStatic = new List<GameObject>(); // Хранятся список статических  игровых фигру
    public static List<GameObject>  AllCollected = new List<GameObject>(); // Хранятся список угаданных игровых фигру
    public GameObject Finger;
    public static int WaitHint = 0;
    int HintTime =0;
    int check = 0;
    public static GameObject Level1Spawn;
    
    void Awake()
    {
//Перемешивания списка.
        WinBobbles.Victory = AllAnimals.Count;
        for (int i = 0; i < AllAnimals.Count; i++)
        {
            int chance = Random.Range(0,AllAnimals.Count-1);
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
    public IEnumerator StartHint()
    {
        while(WinBobbles.Victory != 0)
        {
            while(HintTime < 4 && check == 0)
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
        check = 0;
        string Tag = "";

        foreach (var item in GetComponent<Level1Spawn>().SpawnPosition)
        {
            if(item.activeSelf)
            {
                Tag = item.name;
                Start = item.transform.position;
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
        check = 0;
        Finger.transform.position = new Vector3 (0,10,0);
    }
}