using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Level6
{
    /// <summary>
    /// Управляет основной логикой 6-го уровня.
    /// Наследуется от BaseLevelManager для использования общей логики уровней.
    /// </summary>
    public class Level6Manager : BaseLevelManager<Level6Manager>
    {
        [Header("Настройки уровня 6")]
        [Tooltip("Спаунер для этого уровня.")]
        [SerializeField] private Level6Spawner level6Spawner;
        [HideInInspector] public List<GameObject> collectedStars = new();
        private bool _isVictoryTriggered;

        protected override void Awake()
        {
            base.Awake();
            if (!level6Spawner) level6Spawner = GetComponent<Level6Spawner>();
        }

        protected override void Start()
        {
            WinBobbles.instance?.SetVictoryCondition(15);
            base.Start();
        }

        private void Update()
        {
            if (_isVictoryTriggered || WinBobbles.instance.victoryCondition != 0) return;
            _isVictoryTriggered = true;
            StartCoroutine(WinAnimation());
        }

        /// <summary>
        /// Инициализирует спаунер.
        /// </summary>
        protected override void InitializeSpawner()
        {
            if (level6Spawner)
            {
                level6Spawner.Initialization();
            }
            else
            {
                Debug.LogError("Level6Spawner не назначен или не найден!");
            }
        }

        /// <summary>
        /// Инициализирует систему подсказок.
        /// </summary>
        protected override void InitializeHint()
        {
            if (hint && level6Spawner)
            {
                hint.comparisonType = HintComparisonType.ByTag;
                hint.Initialization(allTargets, level6Spawner.activeItem);
                StartCoroutine(hint.StartHint());
            }
            else
            {
                Debug.LogError("Компонент Hint или Level6Spawner не найден!");
            }
        }

        /// <summary>
        /// Победная анимация для всех собранных звезд.
        /// </summary>
        private IEnumerator WinAnimation()
        {
            if (hint)
            {
                hint.StopAllCoroutines();
                if (hint.finger)
                {
                    hint.finger.SetActive(false);
                }
            }

            yield return new WaitForSeconds(0.5f);

            foreach (var star in collectedStars)
            {
                if (!star || !star.TryGetComponent<WinUp>(out var winUp)) continue;
                StartCoroutine(winUp.Win());
                yield return new WaitForSeconds(0.05f);
            }
        }

        /// <summary>
        /// Обрабатывает неудачный бросок для Уровня 6.
        /// </summary>
        protected override void OnFailedDrop(GameObject draggedObject)
        {
            if (!draggedObject.TryGetComponent<MoveItem>(out var moveItem)) return;
            moveItem.state = 1;
            StartCoroutine(moveItem.Rotation());
        }

        /// <summary>
        /// Обрабатывает успешное перетаскивание для Уровня 6.
        /// </summary>
        protected override void OnSuccessfulDrop(GameObject draggedObject, Collider2D targetCollider, Vector3 startPosition)
        {
            var chest = targetCollider.GetComponent<Level6Chest>();
            if (!chest || chest.busyPlaces >= chest.starPlaceholders.Count)
            {
                if (!draggedObject.TryGetComponent<MoveItem>(out var moveItem)) return;
                draggedObject.transform.position = moveItem.startPosition;
                if (moveItem.state != 0) return;
                moveItem.state = 1;
                StartCoroutine(moveItem.Rotation());
                return;
            }

            var starToShow = chest.starPlaceholders[chest.busyPlaces];
            if (starToShow)
            {
                starToShow.SetActive(true);
                collectedStars.Add(starToShow);
            }

            ProcessSuccessfulPlacement(draggedObject, starToShow.gameObject);
            chest.busyPlaces++;
            if (level6Spawner)
            {
                level6Spawner.RespawnStar(draggedObject);
            }
        }
    }
}