using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5Global : MonoBehaviour
{
    public List<GameObject> EmptyFigures;// пустые формы для расстановки
    public List<GameObject> ColarFigures;// цветные формы для устриц
    public static List<GameObject> NewColarFigures;// Статические цветные формы для устриц
    public List<GameObject> EmptyFiguresVector2;// Точки спавная для пустых форм
    public static List<GameObject> ReadyFigures;// Установленные цветные формы в пустые формы.
    public static List<GameObject> ReadyEmptyFigures;// Установленные пустые ячейкиж
    public List<Sprite> StageOyster;// Спрайты для анимации устриц
    public static List<Sprite> NewStageOyster;// Статические спрайты для устриц
    public static GameObject Delete;// Усисок объектов для отключения вконце
    public GameObject Chest;// Заркытий сундук 
    public GameObject OpenChest;// Открытый сундук
    int Test = 0;//Ограничитель. Чтобы победа была только дин раз
    public GameObject Finger; // Палец для подсказки
    public static int WaitHint = 0; // Отключение подсказки. Получается из MouseClick
    int HintTime = 0; // Время до подсказки.
    public Transform _scale;
    void Awake()
    {
        ReadyEmptyFigures = new List<GameObject>();
        ReadyFigures = new List<GameObject>();
        WinBobbles.Victory = ColarFigures.Count;
        NewStageOyster = StageOyster;
        Delete = GameObject.Find("Delete");
    }
    void Start()
    {
        for (int i = 0; i < ColarFigures.Count; i++)
        {
            int chance = Random.Range(0,11);
            var item = ColarFigures[i];
            ColarFigures[i] = ColarFigures[chance];
            ColarFigures[chance] = item;
        }
        for (int i = 0; i < EmptyFigures.Count; i++)
        {
            int chance = Random.Range(0,11);
            var item = EmptyFigures[i];
            EmptyFigures[i] = EmptyFigures[chance];
            EmptyFigures[chance] = item;
        }
        for (int i = 0; i < EmptyFiguresVector2.Count; i++)
        {
           var Empty = Instantiate(EmptyFigures[i], EmptyFiguresVector2[i].transform.position, EmptyFigures[i].transform.rotation, Level5Global.Delete.transform);
           ReadyEmptyFigures.Add(Empty);
           Empty.transform.localScale = _scale.localScale;
        }
        NewColarFigures = ColarFigures;
        StartCoroutine(StartHint());
    }

    void Update()
    {
        if(WinBobbles.Victory == 0 && Test == 0)
        {
            Test = 1;
            StartCoroutine(DestroyAll());
        }
    }
    IEnumerator DestroyAll()
    {
        Transform[] allChildren = Level5Global.Delete.GetComponentsInChildren<Transform>();
        for (int i = 1; i < allChildren.Length; i++)
        {
            allChildren[i].GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.05f);
        }
        Chest.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(1.5f);
        Chest.SetActive(false);
        OpenChest.SetActive(true);
    }
    public IEnumerator StartHint()
    {
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
        Vector3 Start = new Vector3(0,10,0);
        Vector3 End = new Vector3(0,10,0);
        int check = 0;
        string Tag = "";

        foreach (var item in ReadyFigures)
        {
            if(item != null)
            {
                Tag = item.tag;
                Start = item.transform.position;
                Start.z = 0.0f;
                check = 1;
                break;
            }
        }
        if(check == 1)
        {
            foreach (var item in ReadyEmptyFigures)
            {
                if(Start != null && Tag == item.tag)
                {
                    End = item.transform.position;
                    End.z = 0.0f;
                    check = 2;
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