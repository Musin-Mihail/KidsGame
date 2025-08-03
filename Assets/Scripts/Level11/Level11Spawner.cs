using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using InputController;
using UnityEngine;

namespace Level11
{
    /// <summary>
    /// Спаунер для 11-го уровня. Отвечает за создание сундуков.
    /// Может как использовать заранее заданные точки спауна, так и генерировать их в виде сетки.
    /// </summary>
    public class Level11Spawner : BaseSpawner
    {
        [Header("Настройки генерации сетки")]
        [Tooltip("Количество столбцов в сетке")]
        [SerializeField] private int columns = 8;
        [Tooltip("Количество строк в сетке")]
        [SerializeField] private int rows = 4;
        [Tooltip("Горизонтальное расстояние между точками спауна")]
        [SerializeField] private float horizontalSpacing = 2.1f;
        [Tooltip("Вертикальное расстояние между точками спауна")]
        [SerializeField] private float verticalSpacing = 2.1f;
        [Tooltip("Начальная позиция для генерации сетки (левый верхний угол)")]
        [SerializeField] private Vector2 startPosition = new Vector2(-7.35f, 3.5f);

        [HideInInspector] public List<GameObject> chestsToSpawn;

        /// <summary>
        /// Инициализация спаунера. Создает начальный набор сундуков.
        /// </summary>
        public override void Initialization()
        {
            GenerateSpawnPositions();
            StartCoroutine(SpawnChestsCoroutine());
        }

        /// <summary>
        /// Программно генерирует точки спауна в виде сетки.
        /// </summary>
        private void GenerateSpawnPositions()
        {
            foreach (var pos in startSpawnPositions.Where(pos => pos))
            {
                Destroy(pos);
            }

            startSpawnPositions.Clear();
            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < columns; col++)
                {
                    var xPos = startPosition.x + col * horizontalSpacing;
                    var yPos = startPosition.y - row * verticalSpacing;
                    var spawnPos = new Vector3(xPos, yPos, 0);
                    var spawnPoint = new GameObject($"SpawnPoint_{row}_{col}")
                    {
                        transform =
                        {
                            parent = transform,
                            position = spawnPos
                        }
                    };
                    startSpawnPositions.Add(spawnPoint);
                }
            }
        }

        private IEnumerator SpawnChestsCoroutine()
        {
            var manager = Level11Manager.instance;
            if (!manager) yield break;
            if (startSpawnPositions.Count < chestsToSpawn.Count)
            {
                Debug.LogError("Недостаточно точек спауна для всех сундуков!", this);
                yield break;
            }

            manager.spawnedChests.Clear();
            manager.emptyChestsForDeletion.Clear();
            yield return new WaitForSeconds(1.0f);
            for (var i = 0; i < chestsToSpawn.Count; i++)
            {
                var chestPrefab = chestsToSpawn[i];
                var spawnPosition = startSpawnPositions[i].transform.position;
                var chest = Instantiate(chestPrefab, spawnPosition, Quaternion.identity, parent);
                chest.name = chestPrefab.name;
                chest.tag = chest.name.Contains("Fish") ? "FishChest" : "EmptyChest";
                if (!chest.GetComponent<BoxCollider2D>())
                {
                    chest.AddComponent<BoxCollider2D>();
                }

                if (!chest.GetComponent<Animator>())
                {
                    chest.AddComponent<Animator>();
                }

                manager.spawnedChests.Add(chest);
                if (chest.CompareTag("EmptyChest"))
                {
                    manager.emptyChestsForDeletion.Add(chest);
                }

                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(4.4f);
            GetComponent<ClickController>().enabled = true;
        }
    }
}