using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Level8
{
    /// <summary>
    /// Содержит данные для одного пазла: сам объект, его части и цели.
    /// </summary>
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
    [RequireComponent(typeof(Level8Spawner), typeof(Hint))]
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
        private bool _isCurrentPuzzleFinished;

        protected override void Awake()
        {
            base.Awake();
            if (!level8Spawner) level8Spawner = GetComponent<Level8Spawner>();
        }

        protected override void Start()
        {
            if (WinBobbles.instance)
            {
                WinBobbles.instance.victory = 1;
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
                _currentPuzzle.puzzleObject.SetActive(false);
            }

            if (WinBobbles.instance)
            {
                WinBobbles.instance.victory = 0;
            }
        }

        /// <summary>
        /// Корутина, управляющая процессом сборки одного пазла.
        /// </summary>
        private IEnumerator LevelFlowCoroutine(PuzzleInfo currentPuzzle)
        {
            _isCurrentPuzzleFinished = false;
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
            StartCoroutine(hint.StartHint());
            yield return new WaitUntil(() => _isCurrentPuzzleFinished);
            if (hint)
            {
                hint.StopAllCoroutines();
                if (hint.finger) hint.finger.SetActive(false);
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
        public void OnItemPlaced()
        {
            if (_isCurrentPuzzleFinished) return;
            _itemsToPlaceCount--;
            if (_itemsToPlaceCount <= 0)
            {
                _isCurrentPuzzleFinished = true;
            }

            hint.waitHint = 1;
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
            var activeDraggableItems = new List<GameObject>();
            foreach (var item in level8Spawner.activeItem)
            {
                if (item && item.TryGetComponent<Collider2D>(out var col) && col.enabled)
                {
                    activeDraggableItems.Add(item);
                }
            }

            hint.Initialization(_currentPuzzle.pieceTargets, activeDraggableItems);
        }
    }
}