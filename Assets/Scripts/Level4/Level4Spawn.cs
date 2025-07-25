using System.Linq;
using Core;
using UnityEngine;

namespace Level4
{
    /// <summary>
    /// Спаунер для 4-го уровня. Наследуется от BaseSpawner.
    /// Отвечает за создание и замену перетаскиваемых животных.
    /// </summary>
    public class Level4Spawn : BaseSpawner
    {
        /// <summary>
        /// Инициализация спаунера. Создает начальный набор животных.
        /// </summary>
        public override void Initialization()
        {
            activeItem = new GameObject[startSpawnPositions.Count].ToList();
            for (var i = 0; i < startSpawnPositions.Count; i++)
            {
                SpawnAnimal(i);
            }
        }

        /// <summary>
        /// Создает новое животное на указанной позиции.
        /// </summary>
        /// <param name="index">Индекс в списке startSpawnPositions, куда нужно создать животное.</param>
        private void SpawnAnimal(int index)
        {
            if (Level4Global.instance.allItems.Count <= 0 || index < 0 || index >= activeItem.Count) return;
            var animalPrefab = Level4Global.instance.allItems[0];
            var spawnPosition = startSpawnPositions[index].transform.position;
            var newAnimal = Instantiate(animalPrefab, spawnPosition, Quaternion.identity, parent);
            newAnimal.name = animalPrefab.name;
            activeItem[index] = newAnimal;
            var moveItem = newAnimal.GetComponent<MoveItem>();
            if (moveItem)
            {
                moveItem.Initialization(startSpawnPositions[index].transform.position, endSpawnPositions[index].transform.position);
                StartCoroutine(moveItem.Move());
            }

            Level4Global.instance.allItems.RemoveAt(0);
        }

        /// <summary>
        /// Находит место размещенного животного и спаунит там новое.
        /// </summary>
        /// <param name="placedAnimal">Животное, которое было только что успешно размещено.</param>
        public void RespawnAnimal(GameObject placedAnimal)
        {
            var index = activeItem.FindIndex(item => item == placedAnimal);

            if (index != -1)
            {
                SpawnAnimal(index);
            }
        }
    }
}