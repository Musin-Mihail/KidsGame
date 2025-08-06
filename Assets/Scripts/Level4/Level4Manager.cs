using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Level4
{
    /// <summary>
    /// Менеджер 4-го уровня.
    /// </summary>
    public class Level4Manager : BaseLevelManager<Level4Manager>
    {
        [Header("Настройки уровня 4")]
        [Tooltip("Спаунер для этого уровня. Должен находиться на том же GameObject.")]
        public Level4Spawner level4Spawn;
        public List<GameObject> collectedItems = new();
        private bool _isVictoryTriggered;

        protected override void Awake()
        {
            base.Awake();
            if (!level4Spawn) level4Spawn = GetComponent<Level4Spawner>();
        }

        protected override void Start()
        {
            WinBobbles.instance?.SetVictoryCondition(10);
            base.Start();
        }

        protected override void InitializeSpawner()
        {
            if (level4Spawn)
            {
                level4Spawn.Initialization();
            }
            else
            {
                Debug.LogError("Level4Spawn не назначен или не найден на объекте Level4Manager!");
            }
        }

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
        protected override void OnSuccessfulDrop(GameObject draggedObject, Collider2D targetCollider, Vector3 startPosition)
        {
            WinBobbles.instance?.OnItemPlaced();
            AudioManager.instance?.PlayClickSound();
            if (level4Spawn) level4Spawn.RespawnAnimal(draggedObject);
            var childObjectTransform = targetCollider.transform.Find(draggedObject.name);
            if (childObjectTransform)
            {
                StartCoroutine(MoveAndActivate(draggedObject, childObjectTransform.gameObject, targetCollider.CompareTag("Water")));
            }
            else
            {
                draggedObject.SetActive(false);
            }

            if (WinBobbles.instance?.victoryCondition != 0 || _isVictoryTriggered) return;
            _isVictoryTriggered = true;
            StartCoroutine(WinAnimation());
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
                SpawnSuccessEffect(objectToMove.transform);
            }

            childToActivate.SetActive(true);
            objectToMove.gameObject.SetActive(false);
        }
    }
}