using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Level4
{
    /// <summary>
    /// Менеджер 4-го уровня. Управляет состоянием уровня, победой и инициализацией.
    /// Наследуется от BaseLevelManager для использования общей логики.
    /// </summary>
    public class Level4Global : BaseLevelManager<Level4Global>
    {
        [Header("Настройки уровня 4")]
        [Tooltip("Спаунер для этого уровня. Должен находиться на том же GameObject.")]
        [SerializeField] private Level4Spawn level4Spawn;

        public List<GameObject> collectedItems = new();
        private bool _isVictoryTriggered;

        protected override void Awake()
        {
            base.Awake();
            if (!level4Spawn) level4Spawn = GetComponent<Level4Spawn>();
        }

        protected override void Start()
        {
            if (WinBobbles.instance)
            {
                WinBobbles.instance.victory = 10;
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
        /// Инициализирует спаунер. Вызывается из BaseLevelManager.Start().
        /// </summary>
        protected override void InitializeSpawner()
        {
            if (level4Spawn)
            {
                level4Spawn.Initialization();
            }
            else
            {
                Debug.LogError("Level4Spawn не назначен или не найден на объекте Level4Global!");
            }
        }

        /// <summary>
        /// Инициализирует систему подсказок. Вызывается из BaseLevelManager.Start().
        /// </summary>
        protected override void InitializeHint()
        {
            if (hint && level4Spawn)
            {
                hint.Initialization(allTargets, level4Spawn.activeItem);
                StartCoroutine(hint.StartHint());
            }
            else
            {
                Debug.LogError("Компонент Hint или Level4Spawn не найден!");
            }
        }

        /// <summary>
        /// Победная анимация для всех собранных предметов.
        /// </summary>
        private IEnumerator WinAnimation()
        {
            if (hint)
            {
                hint.StopAllCoroutines();
            }

            yield return new WaitForSeconds(0.5f);

            foreach (var item in collectedItems)
            {
                if (!item || !item.TryGetComponent<WinUp>(out var winUp)) continue;
                StartCoroutine(winUp.Win());
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}