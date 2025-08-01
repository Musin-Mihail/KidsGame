using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Level7
{
    /// <summary>
    /// Управляет основной логикой 7-го уровня, игровым циклом и состоянием победы.
    /// Наследуется от BaseLevelManager для использования общей логики.
    /// </summary>
    [RequireComponent(typeof(Level7Spawner))]
    public class Level7Manager : BaseLevelManager<Level7Manager>
    {
        [Header("Настройки уровня 7")]
        [Tooltip("Ссылка на спаунер этого уровня")]
        public Level7Spawner level7Spawner;
        [HideInInspector]
        public bool canProceed;
        private int _tasksCompleted = 0;
        private const int TotalTasks = 5;

        protected override void Awake()
        {
            base.Awake();
            if (!level7Spawner) level7Spawner = GetComponent<Level7Spawner>();
        }

        protected override void Start()
        {
            if (WinBobbles.instance)
            {
                WinBobbles.instance.victory = 1;
            }

            base.Start();
            StartCoroutine(GameLoop());
        }

        /// <summary>
        /// Основной игровой цикл, который управляет сменой заданий.
        /// </summary>
        private IEnumerator GameLoop()
        {
            while (_tasksCompleted < TotalTasks)
            {
                canProceed = false;
                Shuffle(allItems);
                level7Spawner.SpawnTaskItems();
                hint.waitHint = 1;
                InitializeHint();
                yield return new WaitUntil(() => canProceed);
                _tasksCompleted++;
                yield return new WaitForSeconds(2.0f);
            }

            level7Spawner.ClearAllItems();
            if (WinBobbles.instance)
            {
                WinBobbles.instance.victory = 0;
            }
        }

        /// <summary>
        /// Инициализирует спаунер.
        /// </summary>
        protected override void InitializeSpawner()
        {
            if (level7Spawner)
            {
                level7Spawner.Initialization();
            }
            else
            {
                Debug.LogError("Спаунер для уровня 7 (Level7Spawner) не найден!");
            }
        }

        /// <summary>
        /// Инициализирует и обновляет систему подсказок.
        /// </summary>
        protected override void InitializeHint()
        {
            if (hint && level7Spawner)
            {
                // Находим правильный предмет для подсказки
                var correctItem = level7Spawner.activeItem.Find(item => item && item.name == level7Spawner.currentTarget.name);
                
                if (correctItem)
                {
                    // Инициализируем подсказку для поиска пары для конкретного объекта
                    hint.Initialization(level7Spawner.currentTarget, new List<GameObject> { correctItem });
                }
            }
            else
            {
                Debug.LogError("Компонент Hint или Level7Spawner не настроен!");
            }
        }
    }
}
