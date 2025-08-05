using System.Collections;
using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

// Теперь класс наследуется от нашего универсального Singleton
public class WinBobbles : Singleton<WinBobbles>
{
    [Header("Настройки победы")]
    [Tooltip("Объект, который появляется при победе (черный фон)")]
    public GameObject bgBlack;
    [Tooltip("Префаб пузырька для победной анимации")]
    public GameObject bubble;
    private int _bubblesToWin = 30;
    private bool _winAnimationStarted;
    private readonly WaitForSeconds _winWait = new(1f);
    private readonly WaitForSeconds _loadSceneDelay = new(2.0f);

    /// <summary>
    /// Публичное свойство для чтения текущего состояния победы.
    /// </summary>
    public int victoryCondition { get; private set; }

    /// <summary>
    /// Устанавливает начальное количество условий для победы.
    /// Вызывается из менеджеров уровней.
    /// </summary>
    public void SetVictoryCondition(int count)
    {
        victoryCondition = count;
        _winAnimationStarted = false;
    }

    /// <summary>
    /// Вызывается, когда игрок успешно размещает предмет.
    /// Уменьшает счетчик и проверяет, не достигнуто ли условие победы.
    /// </summary>
    public void OnItemPlaced()
    {
        if (victoryCondition > 0)
        {
            victoryCondition--;
        }

        if (victoryCondition != 0 || _winAnimationStarted) return;
        _winAnimationStarted = true;
        StartCoroutine(Win());
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

        if (_bubblesToWin == 0)
        {
            StartCoroutine(LoadSceneAfterDelay());
        }
    }

    /// <summary>
    /// Корутина, запускающая победную анимацию.
    /// </summary>
    private IEnumerator Win()
    {
        yield return _winWait;
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

    /// <summary>
    /// Загружает сцену после задержки.
    /// </summary>
    private IEnumerator LoadSceneAfterDelay()
    {
        yield return _loadSceneDelay;
        SceneManager.LoadScene("SelectScene");
    }
}