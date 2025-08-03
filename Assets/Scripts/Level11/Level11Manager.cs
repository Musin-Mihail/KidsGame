using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Level11
{
    /// <summary>
    /// Управляет основной логикой 11-го уровня.
    /// Наследуется от BaseLevelManager для использования общей логики уровней.
    /// </summary>
    [RequireComponent(typeof(Level11Spawner), typeof(Hint))]
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
                transform =
                {
                    position = new Vector3(0, -8, 0)
                }
            };
        }

        protected override void Start()
        {
            if (WinBobbles.instance)
            {
                WinBobbles.instance.victory = fishChestCount;
            }

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
        /// Обрабатывает клик по сундуку. Вызывается из LevelDropHandler.
        /// </summary>
        public void OnChestClicked(GameObject chest)
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

        /// <summary>
        /// Уведомляет менеджер о том, что произошло взаимодействие, чтобы сбросить таймер подсказки.
        /// </summary>
        public void NotifyInteraction()
        {
            if (hint)
            {
                hint.waitHint = 1;
            }
        }

        private void HandleFishChest(GameObject chest)
        {
            var animator = chest.GetComponent<Animator>();
            if (animator)
            {
                animator.enabled = false;
            }

            foundFishObjects.Add(chest);
            chest.GetComponent<SpriteRenderer>().sprite = fishSprite;
            var chestCollider = chest.GetComponent<Collider2D>();
            if (chestCollider) chestCollider.enabled = false;
            var stars = Instantiate(Resources.Load<ParticleSystem>("ParticleSrarsLevel11"));
            stars.transform.position = chest.transform.position;
            AudioManager.instance.PlayClickSound();
            StartCoroutine(MoveFishToTarget(chest));
        }

        private void HandleEmptyChest(GameObject chest)
        {
            var bubbles = Instantiate(Resources.Load<ParticleSystem>("BubblesLevel1"));
            bubbles.transform.position = chest.transform.position;
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
            if (!WinBobbles.instance) yield break;
            WinBobbles.instance.victory--;
            if (WinBobbles.instance.victory == 0)
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
            foreach (var animator in from item in foundFishObjects where item select item.GetComponent<Animator>())
            {
                if (animator)
                {
                    animator.enabled = true;
                    animator.Play("Scale");
                }

                yield return new WaitForSeconds(0.02f);
            }
        }
    }
}