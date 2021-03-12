using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
// using System;

public class Level10 : MonoBehaviour
{
    public List<GameObject>  AllItem = new List<GameObject>(); // Прифабы
    public List<GameObject>  AllTarget = new List<GameObject>(); // Корзины
    public List<GameObject>  AllSpawn = new List<GameObject>(); // Точки для спавна
    public List<GameObject>  AllPlace; // Количество занятых мест
    public static List<GameObject> AllBusyPlace = new List<GameObject>(); // Точки для спавна
    public List<float>  AllScale = new List<float>();
    public static int next = 3;
    int HintTime = 0;
    public GameObject Finger;
    public static int WaitHint = 0;
    Vector3 StartPosition;
    Vector3 EndPosition;
    private IEnumerator _startHint;

    void Start()
    {
        AllBusyPlace.Clear();
        _startHint = StartHint();
        next = 3;
        for (int i = 0; i < AllItem.Count; i++)
        {
            int chance = Random.Range(0,AllItem.Count-1);
            var item = AllItem[i];
            AllItem[i] = AllItem[chance];
            AllItem[chance] = item;
        }
        WinBobbles.Victory = 1;
        AllPlace = new List<GameObject>();
        StartCoroutine(StartGame());
        StartCoroutine(_startHint);
    }
    public IEnumerator StartGame()
    {
        for (int item = 0; item < AllItem.Count; item++)
        {
            for (int i = 0; i < AllScale.Count; i++)
            {
                int chance = Random.Range(0,AllScale.Count-1);
                float scale = AllScale[i];
                AllScale[i] = AllScale[chance];
                AllScale[chance] = scale;
            }
            for (int i = 0; i < 3; i++)
            {
                var _item = Instantiate (AllItem[item], AllSpawn[i].transform.position, Quaternion.identity);
                _item.name = AllItem[item].name;
                AllPlace.Add(_item);
                _item.transform.localScale = new Vector3(AllScale[i],AllScale[i],1);
            }
            while(next != 0)
            {
                yield return new WaitForSeconds(0.5f);
            }
            next = 3;
            yield return new WaitForSeconds(1.0f);
            foreach (var _item in AllPlace)
            {
                Destroy(_item);
            }
            AllPlace.Clear();
            WaitHint = 1;
        }
        StopCoroutine(_startHint);

        foreach (var item in AllBusyPlace)
        {
            StartCoroutine(item.GetComponent<WinUp>().Win());
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(WinBobbles.Win());
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
        float scale = 0.0f;
        foreach (var item in AllPlace)
        {
            if(item != null)
            {
                StartPosition = item.transform.position;
                scale = item.transform.localScale.x;
                break;
            }
        }
        foreach (var item in AllTarget)
        {
            if(item.transform.localScale.x == scale)
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