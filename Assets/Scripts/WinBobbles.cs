using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinBobbles : MonoBehaviour
{
    public static int Victory;
    int Stop = 0;
    static public int Count = 30;
    // int count;
    // void Start()
    // {
        // count = 0;
        // StartCoroutine(Screenshot2());
    // }
    void Update()
    {
        if (Victory == 0 && Stop == 0)
        {
            Stop = 1;
            StartCoroutine(WinBobbles.Win());
        }
        if (Count == 0)
        {
            Count = 30;
            Invoke("LoadScene", 2.0f);
        }
        // if(Input.GetKeyDown(KeyCode.S))
        // {
        //     ScreenCapture.CaptureScreenshot(SceneManager.GetActiveScene().name + "_" + Screen.width + "x"+  Screen.height + ".png", 1);
        //     Debug.Log("Скриншот");
        // }
    }
    static public IEnumerator Win()
    {
        yield return new WaitForSeconds(1);
        Direction.BGBlackStatic.SetActive(true);
        float _y = -7.0f;
        var _bubble = Resources.Load<GameObject>("Bubble");
        for (int i = 0; i < 31; i++)
        {
            var NewVector = new Vector3(Random.Range(-6.0f,6.0f), _y, 0);
            _y -= 0.3f;
            var GO = Instantiate(_bubble,NewVector, Quaternion.identity);
            float newRandomScale = Random.Range(0.3f,0.6f);
            var newScale = new Vector3(newRandomScale, newRandomScale, 1);
            GO.transform.localScale = newScale;
            // GO.transform.parent = Direction.CanvasBubblesStatic.transform;
            yield return new WaitForSeconds(Random.Range(0.0f, 0.5f));
        }
    }
    void LoadScene()
    {
        SceneManager.LoadScene("SellectScene");
    }
    // IEnumerator Screenshot2()
    // {
    //     while(true)
    //     {
    //         yield return new WaitForSeconds(1);
    //         count++;
    //         ScreenCapture.CaptureScreenshot(SceneManager.GetActiveScene().name + "_" + Screen.width + "x"+  Screen.height + "_" + count + ".png", 1);
    //         Debug.Log("Скриншот" + count);
    //     }
    // }
}