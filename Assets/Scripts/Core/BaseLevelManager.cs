using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Абстрактный базовый класс для всех менеджеров уровней.
    /// </summary>
    public abstract class BaseLevelManager<T> : Singleton<T> where T : MonoBehaviour
    {
        [Header("Общие настройки уровня")]
        [Tooltip("Компонент подсказки на этом же объекте")]
        [SerializeField] protected Hint hint;

        [Header("Настройки эффектов")]
        [Tooltip("Префаб основного эффекта, который появляется при успешном действии")]
        [SerializeField] protected GameObject successEffectPrefab;
        [Tooltip("Слой сортировки для основного эффекта")]
        [SerializeField] protected int successEffectLayer = 5;

        [Tooltip("Префаб вторичного эффекта (например, при неправильном действии)")]
        [SerializeField] protected GameObject secondaryEffectPrefab;
        [Tooltip("Слой сортировки для вторичного эффекта")]
        [SerializeField] protected int secondaryEffectLayer = 5;

        [Header("Объекты уровня")]
        [Tooltip("Список всех перетаскиваемых предметов на уровне (животные, фигуры и т.д.)")]
        public List<GameObject> allItems = new();
        [Tooltip("Список всех целевых пустых мест на уровне")]
        public List<GameObject> allTargets = new();

        protected override void Awake()
        {
            base.Awake();
            if (!hint) hint = GetComponent<Hint>();
        }

        protected virtual void Start()
        {
            Shuffle(allItems);
            InitializeSpawner();
            InitializeHint();
        }

        /// <summary>
        /// Централизованный метод для обработки успешного размещения объекта.
        /// Воспроизводит звук, создает эффект и деактивирует перетаскиваемый объект.
        /// </summary>
        protected virtual void ProcessSuccessfulPlacement(GameObject draggedObject, GameObject targetObject)
        {
            AudioManager.instance?.PlayClickSound();
            WinBobbles.instance?.OnItemPlaced();
            SpawnSuccessEffect(targetObject.transform);
            draggedObject.SetActive(false);
        }

        /// <summary>
        /// Создает основной эффект в указанной позиции.
        /// </summary>
        protected void SpawnSuccessEffect(Transform transform)
        {
            if (successEffectPrefab)
            {
                var effect = Instantiate(successEffectPrefab, transform.position, successEffectPrefab.transform.rotation);
                if (effect.TryGetComponent<Renderer>(out var effectRenderer))
                {
                    effectRenderer.sortingOrder = successEffectLayer;
                }
            }
            else
            {
                Debug.LogWarning("Префаб основного эффекта (successEffectPrefab) не назначен в инспекторе!", this);
            }
        }

        /// <summary>
        /// Создает вторичный эффект в указанной позиции.
        /// </summary>
        protected void SpawnSecondaryEffect(Transform transform)
        {
            if (secondaryEffectPrefab)
            {
                var effect = Instantiate(secondaryEffectPrefab, transform.position, secondaryEffectPrefab.transform.rotation);
                if (effect.TryGetComponent<Renderer>(out var effectRenderer))
                {
                    effectRenderer.sortingOrder = secondaryEffectLayer;
                }
            }
            else
            {
                Debug.LogWarning("Префаб вторичного эффекта (secondaryEffectPrefab) не назначен в инспекторе!", this);
            }
        }

        /// <summary>
        /// Перемешивает элементы в списке.
        /// </summary>
        protected void Shuffle<TList>(List<TList> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var randomIndex = Random.Range(i, list.Count);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }

        protected abstract void InitializeSpawner();
        protected abstract void InitializeHint();
    }
}