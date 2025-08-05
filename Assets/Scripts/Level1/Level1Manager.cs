using Core;
using InputController;
using UnityEngine;

namespace Level1
{
    public class Level1Manager : BaseLevelManager<Level1Manager>
    {
        [HideInInspector] public Level1Spawner level1Spawn;

        [Header("Контроллеры ввода")]
        [Tooltip("Контроллер для перетаскивания. Необходим для уровней с Drag & Drop.")]
        [SerializeField] private DragAndDropController dragController;

        protected override void Awake()
        {
            base.Awake();
            level1Spawn = GetComponent<Level1Spawner>();
            if (!dragController) dragController = GetComponent<DragAndDropController>();
        }

        private void OnEnable()
        {
            if (dragController)
            {
                dragController.OnSuccessfulDrop += HandleLevel1Drop;
            }
        }

        private void OnDisable()
        {
            if (dragController)
            {
                dragController.OnSuccessfulDrop -= HandleLevel1Drop;
            }
        }

        protected override void Start()
        {
            if (WinBobbles.instance)
            {
                WinBobbles.instance.victory = 8;
            }

            base.Start();
        }

        protected override void InitializeSpawner()
        {
            if (level1Spawn)
            {
                level1Spawn.Initialization();
            }
        }

        protected override void InitializeHint()
        {
            if (!hint || !level1Spawn) return;
            hint.Initialization(allTargets, level1Spawn.activeItem);
            StartCoroutine(hint.StartHint());
        }

        private void HandleLevel1Drop(GameObject draggedObject, Collider2D targetCollider, Vector3 startPosition)
        {
            if (WinBobbles.instance) WinBobbles.instance.victory--;
            var newVector3 = targetCollider.transform.position;
            Instantiate(Resources.Load<ParticleSystem>("BubblesLevel1"), newVector3, Quaternion.Euler(-90, -40, 0));
            targetCollider.GetComponent<SpriteRenderer>().sprite = draggedObject.GetComponent<SpriteRenderer>().sprite;

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
    }
}