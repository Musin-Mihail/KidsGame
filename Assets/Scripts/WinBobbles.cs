using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinBobbles : MonoBehaviour
{
    public static int Victory;
    int Stop = 0;

    // void Start()
    // {
    //     StartCoroutine(Win());
    // }
    void Update()
    {
        if (Victory == 0 && Stop == 0)
        {
            Stop = 1;
            StartCoroutine(WinBobbles.Win());
        }
    }
    
    static public IEnumerator Win()
    {
        yield return new WaitForSeconds(1);
        Direction.BGBlackStatic.SetActive(true);
        for (int i = 0; i < 30; i++)
        {
            var NewVector = new Vector3(Random.Range(-6.0f,6.0f), -8.0f, 0);
            var GO = Instantiate(Resources.Load<GameObject>("Bubble"),NewVector, Quaternion.identity);
            float NewRandom = Random.Range(0.3f,0.6f);
            var NewScale = new Vector3(NewRandom, NewRandom, 1);
            GO.transform.localScale = NewScale;
            // GO.transform.parent = Direction.CanvasBubblesStatic.transform;
            yield return new WaitForSeconds(Random.Range(0.0f, 0.5f));
        }
    }
}