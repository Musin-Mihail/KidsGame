using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinBobbles : MonoBehaviour
{
    public static WinBobbles instance { get; private set; }
    [Header("Настройки победы")]
    [Tooltip("Объект, который появляется при победе (черный фон)")]
    public GameObject bgBlack;
    [Tooltip("Префаб пузырька для победной анимации")]
    public GameObject bubble;
    private int _bubblesToWin = 30;
    private int _stop;

    /// <summary>
    /// Публичное свойство для чтения текущего состояния победы.
    /// </summary>
    public int victoryCondition { get; private set; }

    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    /// <summary>
    /// Устанавливает начальное количество условий для победы.
    /// Вызывается из менеджеров уровней.
    /// </summary>
    public void SetVictoryCondition(int count)
    {
        victoryCondition = count;
    }

    /// <summary>
    /// Вызывается, когда игрок успешно размещает предмет.
    /// </summary>
    public void OnItemPlaced()
    {
        if (victoryCondition > 0)
        {
            victoryCondition--;
        }
    }

    /// <summary>
    /// Вызывается, когда лопается пузырь в победной анимации.
    /// </summary>
    public void OnBubbleBurst()
    {
        if (_bubblesToWin > 0)
        {
            _bubblesToWin--;
        }
    }

    private void Update()
    {
        if (victoryCondition == 0 && _stop == 0)
        {
            _stop = 1;
            StartCoroutine(Win());
        }

        if (_bubblesToWin != 0) return;
        _bubblesToWin = 30;
        Invoke("LoadScene", 2.0f);
    }

    /// <summary>
    /// Корутина, запускающая победную анимацию.
    /// </summary>
    private IEnumerator Win()
    {
        yield return new WaitForSeconds(1);
        if (bgBlack)
        {
            bgBlack.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Объект для фона победы (bgBlack) не назначен в инспекторе!", this);
        }

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