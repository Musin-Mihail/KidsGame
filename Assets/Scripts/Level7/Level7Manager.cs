using System.Collections;
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

        protected override void Awake()
        {
            base.Awake();
            if (!level7Spawner) level7Spawner = GetComponent<Level7Spawner>();
        }

        protected override void Start()
        {
            if (WinBobbles.instance)
            {
                WinBobbles.instance.victory = 5;
            }

            base.Start();
            StartCoroutine(hint.StartHint());
            SetupNextTask();
        }

        /// <summary>
        /// Вызывается из LevelDropHandler после успешного размещения предмета.
        /// </summary>
        public void OnTaskCompleted()
        {
            if (!WinBobbles.instance) return;
            WinBobbles.instance.victory--;
            if (WinBobbles.instance.victory > 0)
            {
                StartCoroutine(WaitAndStartNextTask());
            }
            else
            {
                EndLevel();
            }
        }

        /// <summary>
        /// Корутина для ожидания перед показом нового задания.
        /// </summary>
        private IEnumerator WaitAndStartNextTask()
        {
            yield return new WaitForSeconds(2.0f);
            SetupNextTask();
        }

        /// <summary>
        /// Настраивает и отображает новое задание.
        /// </summary>
        private void SetupNextTask()
        {
            Shuffle(allItems);
            level7Spawner.SpawnTaskItems();
            hint.waitHint = 1;
            InitializeHint();
        }

        /// <summary>
        /// Логика завершения уровня.
        /// </summary>
        private void EndLevel()
        {
            level7Spawner.ClearAllItems();
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
            if (hint && level7Spawner && level7Spawner.currentTarget)
            {
                hint.Initialization(level7Spawner.currentTarget, level7Spawner.activeItem);
            }
            else
            {
                Debug.LogError("Компонент Hint или Level7Spawner не настроен!");
            }
        }
    }
}