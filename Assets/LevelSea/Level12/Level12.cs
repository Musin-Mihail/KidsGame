using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level12 : MonoBehaviour
{
    public List<GameObject> AllTarget = new List<GameObject>();
    public static  List<GameObject> AllTargetStatic = new List<GameObject>();
    public static int count;
    public GameObject Target;
    void Start()
    {
        count = 0;
        WinBobbles.Victory = 8;
        for (int i = 0; i < AllTarget.Count; i++)
        {
            int chance = Random.Range(0,AllTarget.Count-1);
            var item = AllTarget[i];
            AllTarget[i] = AllTarget[chance];
            AllTarget[chance] = item;
        }
        Invoke("StartGame", 5.5f);
    }
    void StartGame()
    {
        Target.GetComponent<Animator>().enabled = false;
        AllTargetStatic = AllTarget;
        AllTargetStatic[0].GetComponent<Animator>().Play("Scale");
    }
    public static void nextFigure()
    {
        AllTargetStatic[count].GetComponent<Animator>().Play("Empty");
        count ++;
        if(count <= 7)
        {
            AllTargetStatic[count].GetComponent<Animator>().Play("Scale");
        }
    }
}