using System.Collections;
using Core;
using Level1;
using Level3;
using Level4;
using Level5;
using Level6;
using UnityEngine;

namespace InputController
{
    /// <summary>
    /// Этот компонент нужно повесить на менеджер уровня.
    /// Он подписывается на событие из DragAndDropController и выполняет логику,
    /// специфичную для конкретного уровня.
    /// </summary>
    public class LevelDropHandler : MonoBehaviour
    {
        [SerializeField] private DragAndDropController dragController;

        private enum LevelType
        {
            Level1,
            Level2,
            Level3,
            Level4,
            Level5,
            Level6
        }
        [SerializeField] private LevelType currentLevel;

        private void OnEnable()
        {
            if (!dragController) return;
            dragController.OnSuccessfulDrop += HandleDrop;
            dragController.OnDropFailed += HandleFailedDrop;
        }

        private void OnDisable()
        {
            if (!dragController) return;
            dragController.OnSuccessfulDrop -= HandleDrop;
            dragController.OnDropFailed -= HandleFailedDrop;
        }

        /// <summary>
        /// Эта функция будет вызвана, когда DragAndDropController зафиксирует успешное размещение.
        /// </summary>
        private void HandleDrop(GameObject draggedObject, Collider2D targetCollider, Vector3 startPosition)
        {
            switch (currentLevel)
            {
                case LevelType.Level1:
                    HandleLevel1Drop(draggedObject, targetCollider);
                    break;
                case LevelType.Level2:
                    HandleLevel2Drop(draggedObject, targetCollider);
                    break;
                case LevelType.Level3:
                    HandleLevel3Drop(draggedObject);
                    break;
                case LevelType.Level4:
                    HandleLevel4Drop(draggedObject, targetCollider);
                    break;
                case LevelType.Level5:
                    HandleLevel5Drop(draggedObject, targetCollider);
                    break;
                case LevelType.Level6:
                    HandleLevel6Drop(draggedObject, targetCollider);
                    break;
            }
        }

        /// <summary>
        /// Обрабатывает неудачный бросок. 
        /// Для 6-го уровня возобновляет вращение для объектов, у которых есть компонент MoveItem.
        /// </summary>
        private void HandleFailedDrop(GameObject draggedObject)
        {
            if (currentLevel != LevelType.Level6) return;
            if (!draggedObject.TryGetComponent<MoveItem>(out var moveItem)) return;
            moveItem.state = 1;
            StartCoroutine(moveItem.Rotation());
        }

        private void HandleLevel1Drop(GameObject draggedObject, Collider2D targetCollider)
        {
            if (WinBobbles.instance) WinBobbles.instance.victory--;
            var newVector3 = targetCollider.transform.position;
            Instantiate(Resources.Load<ParticleSystem>("BubblesLevel1"), newVector3, Quaternion.Euler(-90, -40, 0));
            targetCollider.GetComponent<SpriteRenderer>().sprite = draggedObject.GetComponent<SpriteRenderer>().sprite;
            var level1Spawn = Level1Manager.instance.level1Spawn;
            if (!level1Spawn) return;
            for (var i = 0; i < level1Spawn.activeItem.Count; i++)
            {
                if (!level1Spawn.activeItem[i] || level1Spawn.activeItem[i].name != draggedObject.name) continue;
                level1Spawn.SpawnAnimal(i);
                break;
            }

            AudioManager.instance.PlayClickSound();
            draggedObject.gameObject.SetActive(false);
        }

        private void HandleLevel2Drop(GameObject draggedObject, Collider2D targetCollider)
        {
            if (WinBobbles.instance) WinBobbles.instance.victory--;
            if (targetCollider.name == "Flag")
            {
                targetCollider.GetComponent<Animator>().enabled = true;
            }
            else
            {
                targetCollider.GetComponent<SpriteRenderer>().sprite = draggedObject.GetComponent<SpriteRenderer>().sprite;
            }

            AudioManager.instance.PlayClickSound();
            draggedObject.gameObject.SetActive(false);
        }

        private void HandleLevel3Drop(GameObject draggedObject)
        {
            if (WinBobbles.instance) WinBobbles.instance.victory--;
            draggedObject.gameObject.SetActive(false);
            if (Level3Manager.instance)
            {
                Level3Manager.instance.ChangeFigure();
            }
            else
            {
                Debug.LogError("Level3Global.instance не найден!");
            }

            AudioManager.instance.PlayClickSound();
        }

        private void HandleLevel4Drop(GameObject draggedObject, Collider2D targetCollider)
        {
            if (WinBobbles.instance) WinBobbles.instance.victory--;
            var spawner = Level4Manager.instance.level4Spawn;
            if (spawner) spawner.RespawnAnimal(draggedObject);
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

        private void HandleLevel5Drop(GameObject draggedObject, Collider2D targetCollider)
        {
            AudioManager.instance.PlayClickSound();
            var particlePosition = targetCollider.transform.position;
            Instantiate(Resources.Load<ParticleSystem>("Bubbles"), particlePosition, Quaternion.Euler(-90, -40, 0));
            targetCollider.GetComponent<SpriteRenderer>().sprite = draggedObject.GetComponent<SpriteRenderer>().sprite;
            targetCollider.tag = "Untagged";
            if (WinBobbles.instance) WinBobbles.instance.victory--;
            var oyster = draggedObject.GetComponentInParent<Level5SpawnOyster>();
            if (oyster) StartCoroutine(oyster.SpawnNextFigure());
            var spawner = Level5Manager.instance.GetComponent<Level5Spawner>();
            if (spawner && spawner.activeItem.Contains(draggedObject))
            {
                spawner.activeItem.Remove(draggedObject);
            }

            draggedObject.SetActive(false);
        }

        /// <summary>
        /// Обработчик для логики шестого уровня.
        /// </summary>
        private void HandleLevel6Drop(GameObject draggedObject, Collider2D targetCollider)
        {
            var chest = targetCollider.GetComponent<Level6Chest>();
            if (!chest || chest.busyPlaces >= chest.starPlaceholders.Count)
            {
                if (!draggedObject.TryGetComponent<MoveItem>(out var moveItem)) return;
                draggedObject.transform.position = moveItem.startPosition;
                if (moveItem.state != 0) return;
                moveItem.state = 1;
                StartCoroutine(moveItem.Rotation());

                return;
            }

            AudioManager.instance.PlayClickSound();
            var starToShow = chest.starPlaceholders[chest.busyPlaces];
            if (starToShow)
            {
                starToShow.SetActive(true);
                Level6Manager.instance.collectedStars.Add(starToShow);
                var particlePosition = starToShow.transform.position;
                Instantiate(Resources.Load<ParticleSystem>("Bubbles"), particlePosition, Quaternion.Euler(-90, 0, 0));
            }

            chest.busyPlaces++;
            draggedObject.SetActive(false);
            var spawner = Level6Manager.instance.GetComponent<Level6Spawner>();
            if (spawner)
            {
                spawner.RespawnStar(draggedObject);
            }

            if (WinBobbles.instance)
            {
                WinBobbles.instance.victory--;
            }
        }
    }
}