using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinBobbles : MonoBehaviour
{
    public static WinBobbles Instance { get; private set; }
    public int Victory;
    public int Count = 30;
    public GameObject bubble;
    private int _stop;

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (Victory == 0 && _stop == 0)
        {
            _stop = 1;
            StartCoroutine(Win());
        }

        if (Count == 0)
        {
            Count = 30;
            Invoke("LoadScene", 2.0f);
        }
    }

    public IEnumerator Win()
    {
        yield return new WaitForSeconds(1);
        Direction.Instance.bgBlack.SetActive(true);
        var y = -7.0f;
        for (var i = 0; i < 31; i++)
        {
            var newVector = new Vector3(Random.Range(-6.0f, 6.0f), y, 0);
            y -= 0.3f;
            var go = Instantiate(bubble, newVector, Quaternion.identity);
            var newRandomScale = Random.Range(0.3f, 0.6f);
            var newScale = new Vector3(newRandomScale, newRandomScale, 1);
            go.transform.localScale = newScale;
            yield return new WaitForSeconds(Random.Range(0.0f, 0.5f));
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("SelectScene");
    }
}