using System.Linq;
using Core;
using UnityEngine;

namespace Level6
{
    /// <summary>
    /// Отвечает за создание и замену звезд на уровне.
    /// Наследуется от BaseSpawner.
    /// </summary>
    public class Level6Spawner : BaseSpawner
    {
        /// <summary>
        /// Инициализация спаунера. Создает начальный набор звезд.
        /// </summary>
        public override void Initialization()
        {
            activeItem = new GameObject[startSpawnPositions.Count].ToList();
            for (var i = 0; i < startSpawnPositions.Count; i++)
            {
                SpawnStar(i);
            }
        }

        /// <summary>
        /// Создает новую звезду на указанной позиции.
        /// </summary>
        /// <param name="index">Индекс в списке startSpawnPositions, куда нужно создать звезду.</param>
        private void SpawnStar(int index)
        {
            if (Level6Manager.instance.allItems.Count <= 0 || index < 0 || index >= activeItem.Count) return;
            var starPrefab = Level6Manager.instance.allItems[0];
            var spawnPosition = startSpawnPositions[index].transform.position;
            var newStar = Instantiate(starPrefab, parent, false);
            newStar.name = starPrefab.name;
            newStar.tag = starPrefab.tag;
            activeItem[index] = newStar;
            var moveItem = newStar.GetComponent<MoveItem>();
            if (moveItem)
            {
                moveItem.Initialization(spawnPosition, endSpawnPositions[index].transform.position, GameConstants.DefaultMoveSpeed);
                StartCoroutine(moveItem.Move());
                StartCoroutine(moveItem.Rotation());
            }

            Level6Manager.instance.allItems.RemoveAt(0);
        }

        /// <summary>
        /// Находит место размещенной звезды и спаунит там новую.
        /// </summary>
        /// <param name="placedStar">Звезда, которая была только что успешно размещена.</param>
        public void RespawnStar(GameObject placedStar)
        {
            var index = activeItem.FindIndex(item => item == placedStar);
            if (index != -1)
            {
                SpawnStar(index);
            }
        }
    }
}