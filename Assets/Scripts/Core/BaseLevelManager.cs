using System.Collections.Generic;
using InputController;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Абстрактный базовый класс для всех менеджеров уровней.
    /// Теперь он также обрабатывает подписку на события ввода.
    /// </summary>
    public abstract class BaseLevelManager<T> : Singleton<T> where T : MonoBehaviour
    {
        [Header("Общие настройки уровня")]
        [Tooltip("Компонент подсказки на этом же объекте")]
        [SerializeField] protected Hint hint;

        [Header("Контроллеры ввода")]
        [Tooltip("Контроллер для перетаскивания. Необходим для уровней с Drag & Drop.")]
        [SerializeField] private DragAndDropController dragController;
        [Tooltip("Контроллер для кликов. Необходим для уровней с механикой клика.")]
        [SerializeField] private ClickController clickController;

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
            if (!dragController) dragController = GetComponent<DragAndDropController>();
            if (!clickController) clickController = GetComponent<ClickController>();
        }

        protected virtual void OnEnable()
        {
            if (dragController)
            {
                dragController.OnSuccessfulDrop += HandleSuccessfulDrop;
                dragController.OnDropFailed += HandleFailedDrop;
            }

            if (clickController)
            {
                clickController.OnObjectClicked += HandleClick;
            }
        }

        protected virtual void OnDisable()
        {
            if (dragController)
            {
                dragController.OnSuccessfulDrop -= HandleSuccessfulDrop;
                dragController.OnDropFailed -= HandleFailedDrop;
            }

            if (clickController)
            {
                clickController.OnObjectClicked -= HandleClick;
            }
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
        protected void SpawnSuccessEffect(Transform position)
        {
            if (successEffectPrefab)
            {
                var effect = Instantiate(successEffectPrefab, position.position, successEffectPrefab.transform.rotation);
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
        protected void SpawnSecondaryEffect(Transform position)
        {
            if (secondaryEffectPrefab)
            {
                var effect = Instantiate(secondaryEffectPrefab, position.position, secondaryEffectPrefab.transform.rotation);
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

        /// <summary>
        /// Общий обработчик успешного перетаскивания. Выполняет общую логику и вызывает специфичную для уровня.
        /// </summary>
        private void HandleSuccessfulDrop(GameObject draggedObject, Collider2D targetCollider, Vector3 startPosition)
        {
            if (hint) hint.waitHint = 1;
            OnSuccessfulDrop(draggedObject, targetCollider, startPosition);
        }

        /// <summary>
        /// Общий обработчик неудачного перетаскивания.
        /// </summary>
        private void HandleFailedDrop(GameObject draggedObject)
        {
            OnFailedDrop(draggedObject);
        }

        /// <summary>
        /// Общий обработчик клика.
        /// </summary>
        private void HandleClick(GameObject clickedObject)
        {
            if (hint) hint.waitHint = 1;
            OnClick(clickedObject);
        }

        protected abstract void InitializeSpawner();
        protected abstract void InitializeHint();

        /// <summary>
        /// Метод для реализации специфичной логики уровня при успешном перетаскивании.
        /// </summary>
        protected virtual void OnSuccessfulDrop(GameObject draggedObject, Collider2D targetCollider, Vector3 startPosition)
        {
        }

        /// <summary>
        /// Метод для реализации специфичной логики уровня при неудачном перетаскивании.
        /// </summary>
        protected virtual void OnFailedDrop(GameObject draggedObject)
        {
        }

        /// <summary>
        /// Метод для реализации специфичной логики уровня при клике.
        /// </summary>
        protected virtual void OnClick(GameObject clickedObject)
        {
        }
    }
}