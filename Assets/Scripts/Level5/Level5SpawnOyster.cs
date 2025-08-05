using System.Collections;
using UnityEngine;

namespace Level5
{
    public class Level5SpawnOyster : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Спаунит самую первую фигуру при старте уровня.
        /// </summary>
        public void InitialSpawn()
        {
            StartCoroutine(SpawnNextFigure(isInitial: true));
        }

        /// <summary>
        /// Анимирует устрицу и спаунит следующую фигуру.
        /// </summary>
        public IEnumerator SpawnNextFigure(bool isInitial = false)
        {
            var manager = Level5Manager.instance;
            if (!manager) yield break;
            var spawner = manager.GetComponent<Level5Spawner>();
            if (!spawner) yield break;
            if (!isInitial)
            {
                _spriteRenderer.sprite = manager.oysterStages[1];
                yield return new WaitForSeconds(0.4f);
                _spriteRenderer.sprite = manager.oysterStages[0];
                yield return new WaitForSeconds(0.4f);
            }

            var figureToSpawn = manager.GetNextFigureToSpawn();
            if (!figureToSpawn) yield break;
            _spriteRenderer.sprite = manager.oysterStages[1];
            yield return new WaitForSeconds(0.4f);
            _spriteRenderer.sprite = manager.oysterStages[2];
            var spawnPosition = new Vector3(transform.position.x, transform.position.y, -0.25f);
            var newFigure = Instantiate(figureToSpawn, spawnPosition, figureToSpawn.transform.rotation, transform);
            newFigure.name = figureToSpawn.name;

            if (newFigure.TryGetComponent<MoveItem>(out var moveItem))
            {
                moveItem.Initialization(spawnPosition, spawnPosition, GameConstants.DefaultMoveSpeed);
                moveItem.isMoving = false;
            }

            spawner.activeItem.Add(newFigure);
        }
    }
}