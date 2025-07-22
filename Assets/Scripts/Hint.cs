using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hint : MonoBehaviour
{
    public GameObject finger;

    // Списки для поиска
    private List<GameObject> _itemPositions;
    private List<GameObject> _emptyItemPositions;

    // Поле для прямого указания цели. Если не null, используется логика с поиском пары для этой цели.
    private GameObject _targetObject;

    [HideInInspector] public int waitHint;
    private int _hintTime;

    // Хранит текущую запущенную корутину подсказки, чтобы ее можно было остановить
    private Coroutine _currentHintCoroutine;

    /// <summary>
    /// Инициализация для поиска подсказки по имени в двух списках.
    /// </summary>
    public void Initialization(List<GameObject> newEmptyItemPositions, List<GameObject> newItemPositions)
    {
        _emptyItemPositions = newEmptyItemPositions;
        _itemPositions = newItemPositions;

        // Сбрасываем прямую цель, чтобы использовался поиск по двум спискам
        _targetObject = null;
    }

    /// <summary>
    /// Инициализация для поиска пары для конкретного целевого объекта.
    /// </summary>
    public void Initialization(GameObject targetObject, List<GameObject> newItemPositions)
    {
        _itemPositions = newItemPositions;
        _targetObject = targetObject;

        // Очищаем список пустых позиций, т.к. он не используется в этом режиме
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
                    waitHint = 0; // Сбрасываем флаг после прерывания
                    break;
                }

                _hintTime++;
            }

            if (_hintTime >= 4)
            {
                // Запускаем основную корутину поиска и отображения подсказки
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

        // ПЕРЕКЛЮЧЕНИЕ ЛОГИКИ:
        // Режим 1: Задана конкретная цель. Ищем для нее стартовый объект по имени.
        if (_targetObject != null)
        {
            target = _targetObject;
            if (_itemPositions != null)
            {
                start = _itemPositions.FirstOrDefault(item => item.activeSelf);
            }
        }
        // Режим 2: Цель не задана. Ищем любую возможную пару в двух списках.
        else if (_itemPositions != null && _emptyItemPositions != null)
        {
            // Ищем первый активный объект в списке для поиска
            foreach (var item in _itemPositions.Where(item => item.activeSelf))
            {
                // Ищем для него пустую позицию с таким же именем
                var foundTarget = _emptyItemPositions.FirstOrDefault(empty => empty.name == item.name);
                if (foundTarget != null)
                {
                    start = item;
                    target = foundTarget;
                    break; // Нашли первую возможную пару, выходим
                }
            }
        }

        // Если найдена полная пара (начало и конец), запускаем анимацию
        if (start != null && target != null)
        {
            yield return StartCoroutine(MoveFinger(start, target));
        }
        else
        {
            // Если пара не найдена, просто завершаем корутину
            yield break;
        }
    }

    /// <summary>
    /// Основная корутина, отвечающая за анимацию движения "пальца".
    /// </summary>
    private IEnumerator MoveFinger(GameObject startObject, GameObject targetObject)
    {
        finger.gameObject.SetActive(true);
        finger.transform.position = startObject.transform.position;

        // Корректируем целевую позицию по оси Z
        var adjustedTarget = targetObject.transform.position;
        adjustedTarget.z += -1;

        // Двигаем палец к цели
        while (Vector3.Distance(finger.transform.position, adjustedTarget) > 0.01f)
        {
            // Проверяем, активны ли еще объекты
            if (!startObject.activeInHierarchy || !targetObject.activeInHierarchy)
            {
                break; // Прерываем, если один из объектов стал неактивным
            }

            finger.transform.position = Vector3.MoveTowards(finger.transform.position, adjustedTarget, Time.deltaTime * GameConstants.HintDistance);
            if (waitHint == 1)
            {
                // Если пришел сигнал прервать подсказку, выходим из цикла
                break;
            }

            yield return null; // Используем yield return null для обновления каждый кадр
        }

        // Сбрасываем флаг и выключаем палец
        waitHint = 0;
        finger.gameObject.SetActive(false);
        _currentHintCoroutine = null; // Сбрасываем корутину после завершения
    }
}