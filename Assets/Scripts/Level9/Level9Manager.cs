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

        protected override void Awake()
        {
            base.Awake();
            if (!level9Spawner) level9Spawner = GetComponent<Level9Spawner>();
        }

        protected override void Start()
        {
            WinBobbles.instance?.SetVictoryCondition(allItems.Count);
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
        private void UpdateHintAfterPlacement()
        {
            if (!hint) return;
            hint.waitHint = 1;
            InitializeHint();
        }

        /// <summary>
        /// Обрабатывает успешное перетаскивание для Уровня 9.
        /// </summary>
        protected override void OnSuccessfulDrop(GameObject draggedObject, Collider2D targetCollider, Vector3 startPosition)
        {
            if (!level9Spawner) return;
            ProcessSuccessfulPlacement(draggedObject, targetCollider.gameObject);
            level9Spawner.RespawnItem(draggedObject);
            UpdateHintAfterPlacement();
        }
    }
}