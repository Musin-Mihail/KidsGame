using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level8 : MonoBehaviour
{
    public List<GameObject> AllItem = new List<GameObject>(); // Хранятся куски пазла
    public List<GameObject> AllPlace = new List<GameObject>(); // Хранятся правильные места пазла
    public List<GameObject> AllSpawn = new List<GameObject>(); // Хранятся места для кусков вне пазла.
    public int end = 0;
    public int CountItem;
    public Sprite BaseSprite;
    public GameObject NextAnimal;
    
    void Start()
    {
        WinBobbles.Victory = 1; // Чтобы уровень не завершился
        StartCoroutine(Move());
    }
    IEnumerator Move()
    {
        Vector3 Center = new Vector3(0,0,0);
        Vector3 EndVector = new Vector3(-18,0,0);
        GetComponent<Animator>().enabled = true;
        while(transform.position != Center)
        {
            transform.position = Vector3.MoveTowards(transform.position, Center, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.5f);
        GetComponent<Animator>().enabled = false;
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (var item in AllItem)
        {
            item.GetComponent<SpriteRenderer>().enabled = true;
        }
        GetComponent<SpriteRenderer>().sprite = BaseSprite;
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = new Color32 (255,255,255,120);
        for (int i = 0; i < AllItem.Count; i++)
        {
            StartCoroutine(AllItem[i].GetComponent<Level8MoveItem>().Move(i));
        }
        while(end != 1)
        {
            yield return new WaitForSeconds(0.5f);
        }
        foreach (var item in AllItem)
        {
            item.GetComponent<SpriteRenderer>().enabled = false;
        }
        GetComponent<SpriteRenderer>().color = new Color32 (255,255,255,255);
        GetComponent<Animator>().enabled = true;
        while(transform.position != EndVector)
        {
            transform.position = Vector3.MoveTowards(transform.position, EndVector, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        if(NextAnimal != null)
        {
            NextAnimal.SetActive(true);
        }
        else
        {
            StartCoroutine(WinBobbles.Win());
        }
        // gameObject.SetActive(false);
    }
}