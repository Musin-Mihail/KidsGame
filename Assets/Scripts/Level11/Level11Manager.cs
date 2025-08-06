using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Level11
{
    /// <summary>
    /// Управляет основной логикой 11-го уровня.
    /// </summary>
    public class Level11Manager : BaseLevelManager<Level11Manager>
    {
        [Header("Настройки уровня 11")]
        [Tooltip("Префаб сундука с рыбой")]
        [SerializeField] private GameObject fishChestPrefab;
        [Tooltip("Префаб пустого сундука")]
        [SerializeField] private GameObject emptyChestPrefab;
        [Tooltip("Спрайт рыбы, который показывается после открытия сундука")]
        [SerializeField] private Sprite fishSprite;
        [Tooltip("Количество сундуков с рыбой")]
        [SerializeField] private int fishChestCount = 8;
        [Tooltip("Общее количество сундуков")]
        [SerializeField] private int totalChestCount = 32;
        [Header("Ссылки на компоненты")]
        [Tooltip("Спаунер для этого уровня")]
        public Level11Spawner level11Spawner;
        [HideInInspector] public List<GameObject> spawnedChests = new();
        [HideInInspector] public List<GameObject> emptyChestsForDeletion = new();
        [HideInInspector] public List<GameObject> foundFishObjects = new();
        private GameObject _hintStartObject;

        protected override void Awake()
        {
            base.Awake();
            if (!level11Spawner) level11Spawner = GetComponent<Level11Spawner>();
            _hintStartObject = new GameObject("HintStartObject")
            {
                tag = "FishChest",
                transform = { position = new Vector3(0, -8, 0) }
            };
        }

        private void OnDestroy()
        {
            if (_hintStartObject)
            {
                Destroy(_hintStartObject);
            }
        }

        protected override void Start()
        {
            WinBobbles.instance?.SetVictoryCondition(fishChestCount);
            InitializeSpawner();
            StartCoroutine(WaitForSpawningAndInitializeHint());
        }

        private IEnumerator WaitForSpawningAndInitializeHint()
        {
            while (spawnedChests.Count < totalChestCount)
            {
                yield return null;
            }

            InitializeHint();
            yield return new WaitForSeconds(4.4f);
            if (hint)
            {
                StartCoroutine(hint.StartHint());
            }
        }

        /// <summary>
        /// Инициализирует спаунер.
        /// </summary>
        protected override void InitializeSpawner()
        {
            var chestPrefabs = new List<GameObject>();
            for (var i = 0; i < fishChestCount; i++)
            {
                chestPrefabs.Add(fishChestPrefab);
            }

            for (var i = 0; i < totalChestCount - fishChestCount; i++)
            {
                chestPrefabs.Add(emptyChestPrefab);
            }

            Shuffle(chestPrefabs);
            if (!level11Spawner) return;
            level11Spawner.chestsToSpawn = chestPrefabs;
            level11Spawner.Initialization();
        }

        /// <summary>
        /// Инициализирует компонент подсказки для текущего состояния уровня.
        /// </summary>
        protected override void InitializeHint()
        {
            if (!hint) return;
            hint.comparisonType = HintComparisonType.ByTag;
            var fishChests = spawnedChests
                .Where(c => c && c.CompareTag("FishChest") && c.GetComponent<Collider2D>()?.enabled == true)
                .ToList();
            if (!fishChests.Any())
            {
                hint.Initialization(new List<GameObject>(), new List<GameObject>());
                return;
            }

            var startList = new List<GameObject> { _hintStartObject };
            hint.Initialization(fishChests, startList);
        }

        /// <summary>
        /// Обрабатывает клик по сундуку.
        /// </summary>
        protected override void OnClick(GameObject chest)
        {
            if (chest.CompareTag("FishChest"))
            {
                HandleFishChest(chest);
            }
            else if (chest.CompareTag("EmptyChest"))
            {
                HandleEmptyChest(chest);
            }

            InitializeHint();
        }

        private void HandleFishChest(GameObject chest)
        {
            var animator = chest.GetComponent<Animator>();
            if (animator) animator.enabled = false;
            foundFishObjects.Add(chest);
            chest.GetComponent<SpriteRenderer>().sprite = fishSprite;
            var chestCollider = chest.GetComponent<Collider2D>();
            if (chestCollider) chestCollider.enabled = false;
            AudioManager.instance?.PlayClickSound();
            SpawnSuccessEffect(chest.transform);
            StartCoroutine(MoveFishToTarget(chest));
        }

        private void HandleEmptyChest(GameObject chest)
        {
            SpawnSecondaryEffect(chest.transform);
            emptyChestsForDeletion.Remove(chest);
            Destroy(chest);
        }

        private IEnumerator MoveFishToTarget(GameObject fishObject)
        {
            fishObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
            var targetIndex = foundFishObjects.Count - 1;
            if (targetIndex >= allTargets.Count)
            {
                Debug.LogError("Недостаточно целей (AllTarget) для найденных рыб!", this);
                yield break;
            }

            var targetPosition = allTargets[targetIndex].transform.position;
            while (Vector3.Distance(fishObject.transform.position, targetPosition) > 0.01f)
            {
                fishObject.transform.position = Vector3.MoveTowards(fishObject.transform.position, targetPosition, 10f * Time.deltaTime);
                yield return null;
            }

            fishObject.transform.position = targetPosition;
            allTargets[targetIndex].GetComponent<SpriteRenderer>().enabled = false;

            WinBobbles.instance?.OnItemPlaced();
            if (WinBobbles.instance?.victoryCondition == 0)
            {
                StartCoroutine(WinAnimation());
            }
        }

        private IEnumerator WinAnimation()
        {
            if (hint)
            {
                hint.StopAllCoroutines();
                if (hint.finger) hint.finger.SetActive(false);
            }

            foreach (var item in emptyChestsForDeletion)
            {
                if (item) Destroy(item);
                yield return new WaitForSeconds(0.02f);
            }

            emptyChestsForDeletion.Clear();
            foreach (var animator in foundFishObjects.Select(item => item ? item.GetComponent<Animator>() : null).Where(animator => animator))
            {
                animator.enabled = true;
                animator.Play("Scale");
                yield return new WaitForSeconds(0.02f);
            }
        }
    }
}