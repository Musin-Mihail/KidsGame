using Core;
using UnityEngine;

namespace Level9
{
    /// <summary>
    /// Управляет основной логикой 9-го уровня.
    /// Наследуется от BaseLevelManager для использования общей логики уровней.
    /// </summary>
    public class Level9Manager : BaseLevelManager<Level9Manager>
    {
        [Header("Настройки уровня 9")]
        [Tooltip("Спаунер для этого уровня.")]
        [SerializeField] private Level9Spawner level9Spawner;

        protected override void Start()
        {
            if (WinBobbles.instance)
            {
                WinBobbles.instance.victory = allItems.Count;
            }

            base.Start();
            StartCoroutine(hint.StartHint());
        }

        /// <summary>
        /// Инициализирует спаунер.
        /// </summary>
        protected override void InitializeSpawner()
        {
            if (level9Spawner)
            {
                level9Spawner.Initialization();
            }
            else
            {
                Debug.LogError("Level9Spawner не назначен или не найден!");
            }
        }

        /// <summary>
        /// Инициализирует систему подсказок.
        /// </summary>
        protected override void InitializeHint()
        {
            if (hint && level9Spawner)
            {
                hint.Initialization(allTargets, level9Spawner.activeItem);
            }
            else
            {
                Debug.LogError("Компонент Hint или Level9Spawner не найден!");
            }
        }

        /// <summary>
        /// Вызывается, когда предмет успешно размещен, чтобы обновить подсказку.
        /// </summary>
        public void OnItemPlaced()
        {
            if (!hint) return;
            hint.waitHint = 1;
            InitializeHint();
        }
    }
}