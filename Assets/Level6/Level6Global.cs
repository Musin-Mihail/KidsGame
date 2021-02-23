using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level6Global : MonoBehaviour
{
    public static int Victory = 15;
    public List<GameObject>  AllStars = new List<GameObject>();
    public static List<GameObject>  AllCollectedStars = new List<GameObject>();
    public static List<GameObject>  AllStarsStatic = new List<GameObject>();
    public List<Sprite> SpriteBubble = new List<Sprite>();
    public static List<Sprite> SpriteBubbleStatic = new List<Sprite>();
    public List<GameObject>  AllChest = new List<GameObject>();
    public GameObject Finger;
    public static int WaitHint = 0;
    public int WaitHintPublic;
    int HintTime =0;
    
    void Awake()
    {
        Victory = 15;
        for (int i = 0; i < AllStars.Count; i++)
        {
            int chance = Random.Range(0,11);
            var item = AllStars[i];
            AllStars[i] = AllStars[chance];
            AllStars[chance] = item;
        }
        AllStarsStatic = AllStars;
        SpriteBubbleStatic = SpriteBubble;
        AllCollectedStars = new List<GameObject>();
    }
    void Start() 
    {
        // StartCoroutine(Win());
        // StartCoroutine(Hint());
        StartCoroutine(StartHint());
    }
    void Update()
    {
        WaitHintPublic = WaitHint;
        if (Victory == 0)
        {
            Victory++;
            StartCoroutine(Win());
            StartCoroutine(Win2());
        }
    }
    IEnumerator Win()
    {
        for (int i = 0; i < 10; i++)
        {
            var NewVector = new Vector3(Random.Range(-6.0f,6.0f), -8.0f, 0);
            var GO = Instantiate(Resources.Load<GameObject>("Bubble"),NewVector, Quaternion.identity);
            float NewRandom = Random.Range(0.3f,0.6f);
            var NewScale = new Vector3(NewRandom, NewRandom, 1);
            GO.transform.localScale = NewScale;
            yield return new WaitForSeconds(Random.Range(0.0f, 1.0f));
        }
    }
    IEnumerator Win2()
    {
        foreach (var item in AllCollectedStars)
        {
            StartCoroutine(item.GetComponent<Level6WinStar>().Win());
            yield return new WaitForSeconds(0.05f);
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

        foreach (var item in GetComponent<Level6Spawn>().SpawnPosition)
        {
            if(item!= null && item.transform.position == item.GetComponent<Level6StarMove>().StartPosition)
            {
                Tag = item.tag;
                Start = item.transform.position;
                check = 1;
                break;
            }
        }
        if(check == 1)
        {
            foreach (var item in AllChest)
            {
                if(Start != null && Tag == item.tag)
                {
                    End = item.transform.position;
                    break;
                }
            }
        }
        Start.z = -1;
        Finger.transform.position = Start;
        if(WaitHint != 2)
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