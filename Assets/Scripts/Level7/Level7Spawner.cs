using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Level7
{
    /// <summary>
    /// Отвечает за создание, размещение и очистку объектов для 7-го уровня.
    /// Наследуется от BaseSpawner.
    /// </summary>
    public class Level7Spawner : BaseSpawner
    {
        [Header("Настройки спаунера уровня 7")]
        [Tooltip("Список спрайтов-целей в задании (верхний ряд)")]
        public List<SpriteRenderer> taskTargets = new();
        [Tooltip("Главная цель, куда нужно перетащить предмет")]
        public GameObject currentTarget;
        [Tooltip("Спрайт по умолчанию для главной цели (пустой круг)")]
        [SerializeField] private Sprite defaultTargetSprite;
        private Vector3 _initialTargetScale;

        public override void Initialization()
        {
            if (currentTarget)
            {
                if (!defaultTargetSprite) defaultTargetSprite = currentTarget.GetComponent<SpriteRenderer>().sprite;
                _initialTargetScale = currentTarget.transform.localScale;
            }

            activeItem = new List<GameObject>();
        }

        /// <summary>
        /// Создает новое задание: спаунит 3 предмета и настраивает цели.
        /// </summary>
        public void SpawnTaskItems()
        {
            ClearAllItems();
            if (currentTarget)
            {
                currentTarget.GetComponent<SpriteRenderer>().sprite = defaultTargetSprite;
                currentTarget.transform.localScale = _initialTargetScale;
            }

            var itemsToSpawn = Level7Manager.instance.allItems;
            if (itemsToSpawn.Count < 3)
            {
                Debug.LogError("Недостаточно предметов в allItems для спауна!");
                return;
            }

            for (var i = 0; i < 3; i++)
            {
                var prefab = itemsToSpawn[i];
                var newItem = Instantiate(prefab, parent, false);
                newItem.name = prefab.name;

                var moveItem = newItem.GetComponent<MoveItem>();
                if (moveItem)
                {
                    moveItem.Initialization(startSpawnPositions[i].transform.position, endSpawnPositions[i].transform.position, GameConstants.DefaultMoveSpeed);
                    StartCoroutine(moveItem.Move());
                }

                activeItem.Add(newItem);
            }

            for (var i = 0; i < activeItem.Count; i++)
            {
                var chance = Random.Range(0, activeItem.Count);
                (activeItem[i], activeItem[chance]) = (activeItem[chance], activeItem[i]);
            }

            var item1 = activeItem[1];
            var item2 = activeItem[2];
            taskTargets[0].sprite = item1.GetComponent<SpriteRenderer>().sprite;
            taskTargets[0].transform.localScale = item1.transform.localScale;
            taskTargets[1].sprite = item2.GetComponent<SpriteRenderer>().sprite;
            taskTargets[1].transform.localScale = item2.transform.localScale;
            taskTargets[2].sprite = item1.GetComponent<SpriteRenderer>().sprite;
            taskTargets[2].transform.localScale = item1.transform.localScale;
            taskTargets[3].sprite = item2.GetComponent<SpriteRenderer>().sprite;
            taskTargets[3].transform.localScale = item2.transform.localScale;
            if (currentTarget)
            {
                currentTarget.name = item1.name;
            }
        }

        /// <summary>
        /// Уничтожает все активные перетаскиваемые предметы.
        /// </summary>
        public void ClearAllItems()
        {
            foreach (var item in activeItem.Where(item => item))
            {
                Destroy(item);
            }

            activeItem.Clear();
        }
    }
}