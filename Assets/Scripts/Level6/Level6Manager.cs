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
            if (WinBobbles.instance)
            {
                WinBobbles.instance.victory = 15;
            }

            base.Start();
        }

        private void Update()
        {
            if (_isVictoryTriggered || !WinBobbles.instance || WinBobbles.instance.victory != 0) return;
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
    }
}