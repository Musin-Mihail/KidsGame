using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Level12 : MonoBehaviour
{
    public List<GameObject> AllTarget = new List<GameObject>();
    public static List<GameObject> AllTargetStatic = new List<GameObject>();
    public List<GameObject> AllItem = new List<GameObject>();
    public static List<GameObject> AllItemStatic = new List<GameObject>();
    public static int count;
    public GameObject Target;
    int HintTime;
    public static int WaitHint;
    Vector3 EndPosition;
    public GameObject Finger;
    void Start()
    {
        AllItemStatic = AllItem;
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
        StartCoroutine(StartHint());
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
    public IEnumerator StartHint()
    {
        yield return new WaitForSeconds(5.5f);
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
            foreach (var item in AllItem)
            {
                if (item.name == AllTargetStatic[count].name)
                {
                    EndPosition = item.transform.position;
                    break;
                }
            }
            while(Finger.transform.position != EndPosition)
            {
                Finger.transform.position = Vector3.MoveTowards(Finger.transform.position, EndPosition, 0.1f);
                if(WaitHint == 1)
                {
                    Finger.transform.position = new Vector3 (0,-6,0);
                    break;
                }
                yield return new WaitForSeconds(0.02f);
            }
            Finger.transform.position = new Vector3 (0,-6,0);
        }
    }
}