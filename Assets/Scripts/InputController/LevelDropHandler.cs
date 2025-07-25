// D:\Repositories\KidsGame\Assets\Scripts\InputController\LevelDropHandler.cs

using System.Collections;
using Core;
using Level1;
using Level3;
using Level4;
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
            Level4
        }
        [SerializeField] private LevelType currentLevel;

        private void OnEnable()
        {
            if (dragController)
            {
                dragController.OnSuccessfulDrop += HandleDrop;
            }
        }

        private void OnDisable()
        {
            if (dragController)
            {
                dragController.OnSuccessfulDrop -= HandleDrop;
            }
        }

        /// <summary>
        /// Эта функция будет вызвана, когда DragAndDropController зафиксирует успешное размещение.
        /// </summary>
        /// <param name="draggedObject">Объект, который перетащили.</param>
        /// <param name="targetCollider">Коллайдер цели, на которую поместили объект.</param>
        private void HandleDrop(GameObject draggedObject, Collider2D targetCollider)
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
                    HandleLevel3Drop(draggedObject, targetCollider);
                    break;
                case LevelType.Level4:
                    HandleLevel4Drop(draggedObject, targetCollider);
                    break;
            }
        }

        private void HandleLevel1Drop(GameObject draggedObject, Collider2D targetCollider)
        {
            if (WinBobbles.instance) WinBobbles.instance.victory--;
            Debug.Log(WinBobbles.instance.victory);

            Debug.Log("Обработчик для Уровня 1 сработал!");
            var newVector3 = targetCollider.transform.position;
            Instantiate(Resources.Load<ParticleSystem>("BubblesLevel1"), newVector3, Quaternion.Euler(-90, -40, 0));
            targetCollider.GetComponent<SpriteRenderer>().sprite = draggedObject.GetComponent<SpriteRenderer>().sprite;
            var level1Spawn = Level1Global.instance.level1Spawn;
            if (!level1Spawn) return;
            for (var i = 0; i < level1Spawn.activeItem.Count; i++)
            {
                if (level1Spawn.activeItem[i] == null || level1Spawn.activeItem[i].name != draggedObject.name) continue;
                level1Spawn.SpawnAnimal(i);
                break;
            }

            AudioManager.instance.PlayClickSound(); // <- ИЗМЕНЕНО
            Destroy(draggedObject);
        }

        private void HandleLevel2Drop(GameObject draggedObject, Collider2D targetCollider)
        {
            if (WinBobbles.instance) WinBobbles.instance.victory--;
            Debug.Log("Обработчик для Уровня 2 сработал!");
            if (targetCollider.name == "Flag")
            {
                targetCollider.GetComponent<Animator>().enabled = true;
            }
            else
            {
                targetCollider.GetComponent<SpriteRenderer>().sprite = draggedObject.GetComponent<SpriteRenderer>().sprite;
            }

            AudioManager.instance.PlayClickSound(); // <- ИЗМЕНЕНО
            Destroy(draggedObject);
        }

        private void HandleLevel3Drop(GameObject draggedObject, Collider2D targetCollider)
        {
            if (WinBobbles.instance) WinBobbles.instance.victory--;
            Debug.Log("Обработчик для Уровня 3 сработал!");
            draggedObject.gameObject.SetActive(false);
            if (Level3Global.instance)
            {
                Level3Global.instance.ChangeFigure();
            }
            else
            {
                Debug.LogError("Level3Global.instance не найден! Убедитесь, что объект с этим скриптом существует на сцене.");
            }

            AudioManager.instance.PlayClickSound(); // <- ИЗМЕНЕНО
        }

        /// <summary>
        /// Обработчик для логики четвертого уровня. Проверяет совпадение тегов.
        /// </summary>
        private void HandleLevel4Drop(GameObject draggedObject, Collider2D targetCollider)
        {
            Debug.Log("Обработчик для Уровня 4 сработал!");
            if (!targetCollider.CompareTag(draggedObject.tag))
            {
                return;
            }

            if (WinBobbles.instance)
            {
                WinBobbles.instance.victory--;
            }

            if (targetCollider.CompareTag("Water"))
            {
                var particlePosition = draggedObject.transform.position;
                particlePosition.z += 0.5f;
                Instantiate(Resources.Load<ParticleSystem>("Bubbles"), particlePosition, Quaternion.Euler(-90, -40, 0));
            }

            var spawner = Level4Global.instance.GetComponent<Level4Spawn>();
            if (spawner)
            {
                spawner.RespawnAnimal(draggedObject);
            }

            var childObjectTransform = targetCollider.transform.Find(draggedObject.name);

            if (childObjectTransform)
            {
                AudioManager.instance.PlayClickSound();
                StartCoroutine(MoveAndActivate(draggedObject, childObjectTransform.gameObject));
            }
            else
            {
                Debug.LogWarning($"Не удалось найти дочерний объект с именем '{draggedObject.name}' у цели '{targetCollider.name}'.");
                Destroy(draggedObject);
            }
        }

        /// <summary>
        /// Корутина для плавного перемещения объекта к цели, активации дочернего объекта и удаления исходного.
        /// </summary>
        private IEnumerator MoveAndActivate(GameObject objectToMove, GameObject childToActivate)
        {
            if (objectToMove.TryGetComponent<Collider2D>(out var collider))
            {
                collider.enabled = false;
            }

            const float speed = 15f;
            var targetPosition = childToActivate.transform.position;

            while (Vector3.Distance(objectToMove.transform.position, targetPosition) > 0.01f)
            {
                objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, targetPosition, Time.deltaTime * speed);
                yield return null;
            }

            childToActivate.SetActive(true);
            Destroy(objectToMove);
        }
    }
}