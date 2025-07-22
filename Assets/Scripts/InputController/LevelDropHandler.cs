using Level1;
using Level3;
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
            Level3
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
            targetCollider.GetComponent<SoundClickItem>()?.Play();

            if (WinBobbles.instance)
            {
                WinBobbles.instance.victory--;
            }

            draggedObject.SetActive(false);
            switch (currentLevel)
            {
                case LevelType.Level1:
                    HandleLevel1Drop(draggedObject, targetCollider);
                    break;
                case LevelType.Level2:
                    HandleLevel2Drop(draggedObject, targetCollider);
                    break;
                case LevelType.Level3:
                    HandleLevel3Drop();
                    break;
            }
        }

        private void HandleLevel1Drop(GameObject draggedObject, Collider2D targetCollider)
        {
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
        }

        private void HandleLevel2Drop(GameObject draggedObject, Collider2D targetCollider)
        {
            Debug.Log("Обработчик для Уровня 2 сработал!");
            if (targetCollider.name == "Flag")
            {
                targetCollider.GetComponent<Animator>().enabled = true;
            }
            else
            {
                targetCollider.GetComponent<SpriteRenderer>().sprite = draggedObject.GetComponent<SpriteRenderer>().sprite;
            }
        }

        /// <summary>
        /// Обработчик для логики третьего уровня.
        /// </summary>
        private void HandleLevel3Drop()
        {
            Debug.Log("Обработчик для Уровня 3 сработал!");
            if (Level3Global.instance)
            {
                Level3Global.instance.ChangeFigure();
            }
            else
            {
                Debug.LogError("Level3Global.instance не найден! Убедитесь, что объект с этим скриптом существует на сцене.");
            }
        }
    }
}