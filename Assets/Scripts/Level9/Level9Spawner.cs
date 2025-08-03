using System.Linq;
using Core;
using UnityEngine;

namespace Level9
{
    /// <summary>
    /// Отвечает за создание и замену предметов на уровне.
    /// Наследуется от BaseSpawner.
    /// </summary>
    public class Level9Spawner : BaseSpawner
    {
        /// <summary>
        /// Инициализация спаунера. Создает начальный набор предметов.
        /// </summary>
        public override void Initialization()
        {
            activeItem = new GameObject[startSpawnPositions.Count].ToList();
            for (var i = 0; i < startSpawnPositions.Count; i++)
            {
                SpawnItem(i);
            }
        }

        /// <summary>
        /// Создает новый предмет на указанной позиции.
        /// </summary>
        /// <param name="index">Индекс в списке startSpawnPositions, куда нужно создать предмет.</param>
        private void SpawnItem(int index)
        {
            if (Level9Manager.instance.allItems.Count <= 0 || index < 0 || index >= activeItem.Count) return;
            var itemPrefab = Level9Manager.instance.allItems[0];
            var spawnPosition = startSpawnPositions[index].transform.position;
            var newItem = Instantiate(itemPrefab, parent, false);
            newItem.name = itemPrefab.name;
            newItem.tag = itemPrefab.tag;
            activeItem[index] = newItem;
            if (newItem.TryGetComponent<MoveItem>(out var moveItem))
            {
                moveItem.Initialization(spawnPosition, endSpawnPositions[index].transform.position, GameConstants.DefaultMoveSpeed);
                StartCoroutine(moveItem.Move());
            }
            else
            {
                newItem.transform.position = spawnPosition;
            }

            Level9Manager.instance.allItems.RemoveAt(0);
        }

        /// <summary>
        /// Находит место размещенного предмета и спаунит там новый.
        /// </summary>
        /// <param name="placedItem">Предмет, который был только что успешно размещен.</param>
        public void RespawnItem(GameObject placedItem)
        {
            var index = activeItem.FindIndex(item => item == placedItem);
            if (index == -1) return;
            activeItem[index].SetActive(false);
            SpawnItem(index);
        }
    }
}