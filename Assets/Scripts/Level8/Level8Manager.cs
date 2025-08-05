using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Level8
{
    [Serializable]
    public class PuzzleInfo
    {
        [Tooltip("Основной объект-пазл (животное)")]
        public GameObject puzzleObject;
        [Tooltip("Спрайт, который устанавливается для объекта-пазла после начальной анимации")]
        public Sprite baseSprite;
        [Tooltip("Список префабов перетаскиваемых частей этого пазла")]
        public List<GameObject> puzzlePieces = new();
        [Tooltip("Список целевых мест на этом пазле")]
        public List<GameObject> pieceTargets = new();
        [Tooltip("Список позиций для спауна частей этого пазла")]
        public List<Transform> spawnPositions = new();
    }

    /// <summary>
    /// Управляет основной логикой и последовательностью пазлов на 8-м уровне.
    /// </summary>
    public class Level8Manager : BaseLevelManager<Level8Manager>
    {
        [Header("Настройки уровня 8")]
        [Tooltip("Список всех пазлов, которые нужно собрать на уровне по порядку")]
        [SerializeField] private List<PuzzleInfo> puzzles = new();
        [Header("Компоненты")]
        [Tooltip("Спаунер для этого уровня")]
        [SerializeField] private Level8Spawner level8Spawner;
        private PuzzleInfo _currentPuzzle;
        private int _itemsToPlaceCount;

        protected override void Awake()
        {
            base.Awake();
            if (!level8Spawner) level8Spawner = GetComponent<Level8Spawner>();
        }

        protected override void Start()
        {
            WinBobbles.instance?.SetVictoryCondition(puzzles.Count);
            if (hint)
            {
                StartCoroutine(hint.StartHint());
            }

            StartCoroutine(RunAllPuzzlesSequentially());
        }

        /// <summary>
        /// Главная корутина, которая последовательно запускает каждый пазл из списка.
        /// </summary>
        private IEnumerator RunAllPuzzlesSequentially()
        {
            foreach (var puzzle in puzzles)
            {
                _currentPuzzle = puzzle;
                _currentPuzzle.puzzleObject.SetActive(true);
                yield return StartCoroutine(LevelFlowCoroutine(_currentPuzzle));
                if (_currentPuzzle.puzzleObject)
                {
                    _currentPuzzle.puzzleObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Корутина, управляющая процессом сборки одного пазла.
        /// </summary>
        private IEnumerator LevelFlowCoroutine(PuzzleInfo currentPuzzle)
        {
            _itemsToPlaceCount = currentPuzzle.puzzlePieces.Count;
            var puzzleTransform = currentPuzzle.puzzleObject.transform;
            var puzzleAnimator = currentPuzzle.puzzleObject.GetComponent<Animator>();
            var puzzleRenderer = currentPuzzle.puzzleObject.GetComponent<SpriteRenderer>();
            var startPosition = puzzleTransform.position;
            var centerPosition = Vector3.zero;
            var offScreenPosition = new Vector3(-18, 0, 0);
            puzzleAnimator.enabled = true;
            while (Vector3.Distance(puzzleTransform.position, centerPosition) > 0.01f)
            {
                puzzleTransform.position = Vector3.MoveTowards(puzzleTransform.position, centerPosition, 10f * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
            puzzleAnimator.enabled = false;
            puzzleRenderer.sprite = currentPuzzle.baseSprite;
            yield return new WaitForSeconds(0.5f);
            puzzleRenderer.color = new Color(1, 1, 1, 0.5f);

            InitializeSpawner();
            InitializeHint();

            if (hint) hint.isHintingActive = true;

            while (_itemsToPlaceCount > 0)
            {
                yield return null;
            }

            if (hint)
            {
                hint.isHintingActive = false;
                if (hint.finger && hint.finger.activeSelf)
                {
                    hint.finger.SetActive(false);
                }
            }

            foreach (var item in level8Spawner.activeItem.Where(item => item))
            {
                item.SetActive(false);
            }

            puzzleRenderer.color = Color.white;
            puzzleAnimator.enabled = true;
            yield return new WaitForSeconds(1f);
            while (Vector3.Distance(puzzleTransform.position, offScreenPosition) > 0.01f)
            {
                puzzleTransform.position = Vector3.MoveTowards(puzzleTransform.position, offScreenPosition, 10f * Time.deltaTime);
                yield return null;
            }

            puzzleTransform.position = startPosition;
        }

        /// <summary>
        /// Вызывается из LevelDropHandler, когда предмет успешно размещен.
        /// </summary>
        private void OnItemPlaced()
        {
            if (_itemsToPlaceCount <= 0) return;
            _itemsToPlaceCount--;
            if (_itemsToPlaceCount <= 0)
            {
                WinBobbles.instance?.OnItemPlaced();
            }

            InitializeHint();
        }

        protected override void InitializeSpawner()
        {
            if (!level8Spawner || _currentPuzzle == null) return;
            level8Spawner.piecesToSpawn = _currentPuzzle.puzzlePieces;
            level8Spawner.targets = _currentPuzzle.pieceTargets;
            level8Spawner.spawnPositions = _currentPuzzle.spawnPositions;
            level8Spawner.Initialization();
        }

        protected override void InitializeHint()
        {
            if (!hint || !level8Spawner || _currentPuzzle == null) return;
            var activeDraggableItems = level8Spawner.activeItem
                .Where(item => item && item.TryGetComponent<Collider2D>(out var col) && col.enabled)
                .ToList();
            hint.Initialization(_currentPuzzle.pieceTargets, activeDraggableItems);
        }

        /// <summary>
        /// Обрабатывает успешное перетаскивание для Уровня 8.
        /// </summary>
        protected override void OnSuccessfulDrop(GameObject draggedObject, Collider2D targetCollider, Vector3 startPosition)
        {
            AudioManager.instance?.PlayClickSound();
            SpawnSuccessEffect(draggedObject.transform);
            draggedObject.transform.position = targetCollider.transform.position;
            draggedObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
            var col = draggedObject.GetComponent<Collider2D>();
            if (col) col.enabled = false;
            OnItemPlaced();
        }
    }
}