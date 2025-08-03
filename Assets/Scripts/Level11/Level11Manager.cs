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
    [RequireComponent(typeof(Level11Spawner))]
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

        [Header("Настройки подсказок")]
        [Tooltip("Объект 'пальца' для подсказки")]
        [SerializeField] private GameObject finger;
        [Tooltip("Задержка перед показом подсказки в секундах")]
        [SerializeField] private float hintDelay = 4f;

        [Header("Ссылки на компоненты")]
        [Tooltip("Спаунер для этого уровня")]
        public Level11Spawner level11Spawner;

        [HideInInspector] public List<GameObject> spawnedChests = new();
        [HideInInspector] public List<GameObject> emptyChestsForDeletion = new();
        [HideInInspector] public List<GameObject> foundFishObjects = new();

        private int _waitHint;

        protected override void Awake()
        {
            base.Awake();
            if (!level11Spawner) level11Spawner = GetComponent<Level11Spawner>();
        }

        protected override void Start()
        {
            if (WinBobbles.instance)
            {
                WinBobbles.instance.victory = fishChestCount;
            }

            InitializeSpawner();
            StartCoroutine(HintCoroutine());
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
        /// На этом уровне используется своя логика подсказок, поэтому этот метод не переопределяется.
        /// </summary>
        protected override void InitializeHint()
        {
            // Не используется
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
        }

        /// <summary>
        /// Уведомляет менеджер о том, что произошло взаимодействие, чтобы сбросить таймер подсказки.
        /// </summary>
        public void NotifyInteraction()
        {
            _waitHint = 1;
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

        private IEnumerator HintCoroutine()
        {
            yield return new WaitForSeconds(hintDelay);
            while (WinBobbles.instance && WinBobbles.instance.victory > 0)
            {
                var hintTimer = 0f;
                while (hintTimer < hintDelay)
                {
                    yield return new WaitForSeconds(1.0f);
                    if (_waitHint == 1)
                    {
                        _waitHint = 0;
                        hintTimer = 0;
                        break;
                    }

                    hintTimer++;
                }

                if (hintTimer >= hintDelay)
                {
                    yield return StartCoroutine(ShowHintAnimation());
                }

                yield return new WaitForSeconds(1.0f);
            }
        }

        private IEnumerator ShowHintAnimation()
        {
            if (!finger) yield break;

            var unopenedFishChests = spawnedChests
                .Where(c => c && c.CompareTag("FishChest") && c.GetComponent<Collider2D>() && c.GetComponent<Collider2D>().enabled)
                .ToList();

            if (!unopenedFishChests.Any()) yield break;
            {
                finger.SetActive(true);
                var targetChest = unopenedFishChests.OrderBy(c => Vector3.Distance(finger.transform.position, c.transform.position)).First();
                var targetPosition = targetChest.transform.position;

                while (Vector3.Distance(finger.transform.position, targetPosition) > 0.01f)
                {
                    finger.transform.position = Vector3.MoveTowards(finger.transform.position, targetPosition, Time.deltaTime * GameConstants.HintDistance);
                    if (_waitHint == 1)
                    {
                        break;
                    }

                    yield return null;
                }

                _waitHint = 0;
                finger.SetActive(false);
            }
        }
    }
}