using System.Collections;
using System.Collections.Generic;
using Core;
using InputController;
using UnityEngine;

namespace Level4
{
    /// <summary>
    /// Менеджер 4-го уровня. Управляет состоянием уровня, победой и инициализацией.
    /// Наследуется от BaseLevelManager для использования общей логики.
    /// </summary>
    public class Level4Manager : BaseLevelManager<Level4Manager>
    {
        [Header("Настройки уровня 4")]
        [Tooltip("Спаунер для этого уровня. Должен находиться на том же GameObject.")]
        public Level4Spawner level4Spawn;

        [Header("Контроллеры ввода")]
        [Tooltip("Контроллер для перетаскивания. Необходим для уровней с Drag & Drop.")]
        [SerializeField] private DragAndDropController dragController;

        public List<GameObject> collectedItems = new();
        private bool _isVictoryTriggered;

        protected override void Awake()
        {
            base.Awake();
            if (!level4Spawn) level4Spawn = GetComponent<Level4Spawner>();
            if (!dragController) dragController = GetComponent<DragAndDropController>();
        }

        private void OnEnable()
        {
            if (dragController)
            {
                dragController.OnSuccessfulDrop += HandleLevel4Drop;
            }
        }

        private void OnDisable()
        {
            if (dragController)
            {
                dragController.OnSuccessfulDrop -= HandleLevel4Drop;
            }
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

        /// <summary>
        /// Обрабатывает успешное перетаскивание для Уровня 4.
        /// </summary>
        private void HandleLevel4Drop(GameObject draggedObject, Collider2D targetCollider, Vector3 startPosition)
        {
            if (WinBobbles.instance) WinBobbles.instance.victory--;

            if (level4Spawn) level4Spawn.RespawnAnimal(draggedObject);
            var childObjectTransform = targetCollider.transform.Find(draggedObject.name);
            if (childObjectTransform)
            {
                AudioManager.instance.PlayClickSound();
                StartCoroutine(MoveAndActivate(draggedObject, childObjectTransform.gameObject, targetCollider.CompareTag("Water")));
            }
            else
            {
                draggedObject.gameObject.SetActive(false);
            }
        }

        private IEnumerator MoveAndActivate(GameObject objectToMove, GameObject childToActivate, bool water)
        {
            if (objectToMove.TryGetComponent<Collider2D>(out var objectCollider))
            {
                objectCollider.enabled = false;
            }

            const float speed = 15f;
            var targetPosition = childToActivate.transform.position;
            while (Vector3.Distance(objectToMove.transform.position, targetPosition) > 0.01f)
            {
                objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, targetPosition, Time.deltaTime * speed);
                yield return null;
            }

            if (water)
            {
                var particlePosition = objectToMove.transform.position;
                Instantiate(Resources.Load<ParticleSystem>("Bubbles"), particlePosition, Quaternion.Euler(-90, -40, 0));
            }

            childToActivate.SetActive(true);
            objectToMove.gameObject.SetActive(false);
        }
    }
}