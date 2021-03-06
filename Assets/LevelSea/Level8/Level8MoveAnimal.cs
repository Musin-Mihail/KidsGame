using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level8MoveAnimal : MonoBehaviour
{
    public int end = 0;
    public int CountItem;
    public Sprite BaseSprite;
    void Start()
    {
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
        foreach (var item in allChildren)
        {
            if(item.GetComponent<SpriteRenderer>())
            {
                item.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        GetComponent<SpriteRenderer>().sprite = BaseSprite;
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = new Color32 (255,255,255,120);
        foreach (var item in allChildren)
        {
            if(item.GetComponent<Level8MoveItem>())
            {
                StartCoroutine(item.GetComponent<Level8MoveItem>().Move());
            }
        }
        while(end != 1)
        {
            yield return new WaitForSeconds(0.5f);
        }

        for (int i = 1; i < allChildren.Length; i++)
        {
            if(allChildren[i].GetComponent<SpriteRenderer>())
            {
                allChildren[i].GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        GetComponent<SpriteRenderer>().color = new Color32 (255,255,255,255);
        GetComponent<Animator>().enabled = true;
        while(transform.position != EndVector)
        {
            transform.position = Vector3.MoveTowards(transform.position, EndVector, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        Debug.Log("end");
    }
}
