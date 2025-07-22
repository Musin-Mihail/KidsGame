using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hint : MonoBehaviour
{
    public GameObject finger;

    [HideInInspector] public int waitHint;

    private List<GameObject> _itemPositions;
    private List<GameObject> _emptyItemPositions;
    private GameObject _targetObject;
    private string _targetName;
    private int _hintTime;
    private Coroutine _currentHintCoroutine;

    /// <summary>
    /// Инициализация для поиска подсказки по имени в двух списках.
    /// </summary>
    public void Initialization(List<GameObject> newEmptyItemPositions, List<GameObject> newItemPositions)
    {
        _emptyItemPositions = newEmptyItemPositions;
        _itemPositions = newItemPositions;

        _targetObject = null;
        _targetName = null;
    }

    /// <summary>
    /// Инициализация для поиска пары для конкретного целевого объекта по его имени.
    /// </summary>
    public void Initialization(GameObject targetObject, List<GameObject> newItemPositions)
    {
        _itemPositions = newItemPositions;
        _targetObject = targetObject;
        _targetName = targetObject.name; // Сохраняем имя для поиска

        _emptyItemPositions = null;
    }

    /// <summary>
    /// Запускает автоматическую подсказку после периода бездействия.
    /// </summary>
    public IEnumerator StartHint()
    {
        while (Application.isPlaying && WinBobbles.instance.victory != 0)
        {
            _hintTime = 0;
            while (_hintTime < 4)
            {
                yield return new WaitForSeconds(1.0f);
                if (waitHint == 1)
                {
                    _hintTime = 0;
                    waitHint = 0;
                    break;
                }

                _hintTime++;
            }

            if (_hintTime >= 4)
            {
                ShowHint();
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    /// <summary>
    /// Публичный метод для принудительного запуска подсказки.
    /// </summary>
    private void ShowHint()
    {
        if (_currentHintCoroutine != null)
        {
            StopCoroutine(_currentHintCoroutine);
        }

        _currentHintCoroutine = StartCoroutine(SearchAndShow());
    }

    /// <summary>
    /// Корутина, которая решает, как искать цель, и запускает анимацию.
    /// </summary>
    private IEnumerator SearchAndShow()
    {
        GameObject start = null;
        GameObject target = null;

        if (_targetObject)
        {
            target = _targetObject;
            if (_itemPositions != null && !string.IsNullOrEmpty(_targetName))
            {
                foreach (var itemPosition in _itemPositions)
                {
                    Debug.Log(itemPosition.activeSelf);
                    Debug.Log(itemPosition.name);
                }

                start = _itemPositions.FirstOrDefault(item => item.activeSelf && item.name == _targetName);
                Debug.Log(start);
            }
        }
        else if (_itemPositions != null && _emptyItemPositions != null)
        {
            foreach (var item in _itemPositions.Where(item => item.activeSelf))
            {
                var foundTarget = _emptyItemPositions.FirstOrDefault(empty => empty.name == item.name);
                if (!foundTarget) continue;
                start = item;
                target = foundTarget;
                break;
            }
        }

        if (start && target)
        {
            yield return StartCoroutine(MoveFinger(start, target));
        }
    }

    /// <summary>
    /// Основная корутина, отвечающая за анимацию движения "пальца".
    /// </summary>
    private IEnumerator MoveFinger(GameObject startObject, GameObject targetObject)
    {
        finger.gameObject.SetActive(true);
        finger.transform.position = startObject.transform.position;

        var adjustedTarget = targetObject.transform.position;
        adjustedTarget.z += -1;

        while (Vector3.Distance(finger.transform.position, adjustedTarget) > 0.01f)
        {
            if (!startObject.activeInHierarchy || !targetObject.activeInHierarchy)
            {
                break;
            }

            finger.transform.position = Vector3.MoveTowards(finger.transform.position, adjustedTarget, Time.deltaTime * GameConstants.HintDistance);
            if (waitHint == 1)
            {
                break;
            }

            yield return null;
        }

        waitHint = 0;
        finger.gameObject.SetActive(false);
        _currentHintCoroutine = null;
    }
}