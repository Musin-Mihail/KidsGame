using System.Collections;
using System.Collections.Generic;
using Core;
using InputController;
using UnityEngine;

namespace Level10
{
    /// <summary>
    /// Управляет основной логикой 10-го уровня.
    /// Наследуется от BaseLevelManager для использования общей логики уровней.
    /// </summary>
    [RequireComponent(typeof(Level10Spawner), typeof(Hint))]
    public class Level10Manager : BaseLevelManager<Level10Manager>
    {
        [Header("Настройки уровня 10")]
        [Tooltip("Спаунер для этого уровня")]
        [SerializeField] private Level10Spawner level10Spawner;
        [Tooltip("Список всех возможных масштабов для предметов")]
        public List<float> allScales = new();
        [Tooltip("Список тегов размеров, соответствующих масштабам (например, Small, Medium, Big)")]
        public List<string> allSizes = new();

        [Header("Контроллеры ввода")]
        [Tooltip("Контроллер для перетаскивания. Необходим для уровней с Drag & Drop.")]
        [SerializeField] private DragAndDropController dragController;

        private const int ItemsPerRound = 3;
        private readonly List<GameObject> _placedTargetObjects = new();
        private int _currentItemIndex;
        private int _placedThisRound;

        protected override void Awake()
        {
            base.Awake();
            if (!dragController) dragController = GetComponent<DragAndDropController>();
        }

        private void OnEnable()
        {
            if (dragController)
            {
                dragController.OnSuccessfulDrop += HandleLevel10Drop;
            }
        }

        private void OnDisable()
        {
            if (dragController)
            {
                dragController.OnSuccessfulDrop -= HandleLevel10Drop;
            }
        }

        protected override void Start()
        {
            WinBobbles.instance?.SetVictoryCondition(1);
            Shuffle(allItems);
            StartCoroutine(GameFlow());
            StartCoroutine(hint.StartHint());
        }

        /// <summary>
        /// Основной игровой цикл, который последовательно проходит по всем основным предметам (например, бабочка, цветок и т.д.).
        /// </summary>
        private IEnumerator GameFlow()
        {
            for (_currentItemIndex = 0; _currentItemIndex < allItems.Count; _currentItemIndex++)
            {
                SetupNewRound();
                while (_placedThisRound < ItemsPerRound)
                {
                    yield return null;
                }

                yield return new WaitForSeconds(1.0f);
            }

            yield return StartCoroutine(WinAnimation());
            WinBobbles.instance?.SetVictoryCondition(0);
        }

        /// <summary>
        /// Настраивает новый раунд: спаунит предметы и инициализирует подсказку.
        /// </summary>
        private void SetupNewRound()
        {
            _placedThisRound = 0;
            ShuffleScalesAndSizes();
            InitializeSpawner();
            InitializeHint();
            if (hint)
            {
                hint.waitHint = 1;
            }
        }

        /// <summary>
        /// Корутина для победной анимации.
        /// </summary>
        private IEnumerator WinAnimation()
        {
            if (hint)
            {
                hint.StopAllCoroutines();
                if (hint.finger) hint.finger.SetActive(false);
            }

            foreach (var item in _placedTargetObjects)
            {
                if (!item.TryGetComponent<WinUp>(out var winUp)) continue;
                StartCoroutine(winUp.Win());
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(0.5f);
        }

        /// <summary>
        /// Вызывается при успешном размещении предмета.
        /// </summary>
        /// <param name="targetContainer">Родительский объект (цель), куда был перетащен предмет.</param>
        private void OnItemPlaced(GameObject targetContainer)
        {
            _placedThisRound++;
            foreach (Transform child in targetContainer.transform)
            {
                if (child.name != allItems[_currentItemIndex].name) continue;
                child.GetComponent<SpriteRenderer>().enabled = true;
                _placedTargetObjects.Add(child.gameObject);
                break;
            }

            if (hint)
            {
                hint.waitHint = 1;
            }
        }

        /// <summary>
        /// Перемешивает списки масштабов и размеров синхронно.
        /// </summary>
        private void ShuffleScalesAndSizes()
        {
            for (var i = 0; i < allScales.Count; i++)
            {
                var randomIndex = Random.Range(i, allScales.Count);
                (allScales[i], allScales[randomIndex]) = (allScales[randomIndex], allScales[i]);
                (allSizes[i], allSizes[randomIndex]) = (allSizes[randomIndex], allSizes[i]);
            }
        }

        protected override void InitializeSpawner()
        {
            if (!level10Spawner) return;
            level10Spawner.itemToSpawn = allItems[_currentItemIndex];
            level10Spawner.scales = allScales;
            level10Spawner.sizes = allSizes;
            level10Spawner.Initialization();
        }

        protected override void InitializeHint()
        {
            if (!hint || !level10Spawner) return;
            hint.Initialization(allTargets, level10Spawner.activeItem);
        }

        /// <summary>
        /// Обрабатывает успешное перетаскивание для Уровня 10.
        /// </summary>
        private void HandleLevel10Drop(GameObject draggedObject, Collider2D targetCollider, Vector3 startPosition)
        {
            AudioManager.instance?.PlayClickSound();
            OnItemPlaced(targetCollider.gameObject);
            draggedObject.SetActive(false);
        }
    }
}