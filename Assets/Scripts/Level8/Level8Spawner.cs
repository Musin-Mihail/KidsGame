using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Level8
{
    /// <summary>
    /// Отвечает за начальное размещение перетаскиваемых частей пазла.
    /// </summary>
    public class Level8Spawner : BaseSpawner
    {
        public List<GameObject> piecesToSpawn { get; set; }
        public List<GameObject> targets { get; set; }
        public List<Transform> spawnPositions { get; set; }

        public override void Initialization()
        {
            if (piecesToSpawn == null || this.targets == null || spawnPositions == null || spawnPositions.Count == 0)
            {
                Debug.LogError("Данные для спауна (PiecesToSpawn/Targets/SpawnPositions) не были установлены или список SpawnPositions пуст!");
                return;
            }

            activeItem.Clear();
            var pieces = new List<GameObject>(piecesToSpawn);
            var targetsGo = new List<GameObject>(targets);
            for (var i = 0; i < pieces.Count; i++)
            {
                if (i >= spawnPositions.Count)
                {
                    Debug.LogWarning($"Недостаточно стартовых позиций для спауна. Предметов: {pieces.Count}, позиций: {spawnPositions.Count}");
                    break;
                }

                var pieceToMove = pieces[i];
                pieceToMove.GetComponent<SpriteRenderer>().enabled = true;
                var spawnPos = spawnPositions[i].position;
                if (parent) pieceToMove.transform.SetParent(parent);
                pieceToMove.name = targetsGo[i].name;
                pieceToMove.SetActive(true);
                activeItem.Add(pieceToMove);
                if (pieceToMove.TryGetComponent<MoveItem>(out var moveItem))
                {
                    moveItem.Initialization(pieceToMove.transform.position, spawnPos, GameConstants.Level8MoveSpeed);
                    StartCoroutine(moveItem.Move());
                }
                else
                {
                    pieceToMove.transform.position = spawnPos;
                    Debug.LogWarning($"Объект '{pieceToMove.name}' не имеет компонента MoveItem. Он был размещен мгновенно.");
                }
            }
        }
    }
}