using Core;
using UnityEngine;

namespace Level1
{
    public class Level1Manager : BaseLevelManager<Level1Manager>
    {
        [HideInInspector] public Level1Spawner level1Spawn;

        protected override void Awake()
        {
            base.Awake();
            level1Spawn = GetComponent<Level1Spawner>();
        }

        protected override void Start()
        {
            WinBobbles.instance?.SetVictoryCondition(8);
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

        /// <summary>
        /// Реализуем специфичную для уровня логику, которая вызывается из базового класса.
        /// </summary>
        protected override void OnSuccessfulDrop(GameObject draggedObject, Collider2D targetCollider, Vector3 startPosition)
        {
            ProcessSuccessfulPlacement(draggedObject, targetCollider.gameObject);
            targetCollider.GetComponent<SpriteRenderer>().sprite = draggedObject.GetComponent<SpriteRenderer>().sprite;

            if (!level1Spawn) return;
            for (var i = 0; i < level1Spawn.activeItem.Count; i++)
            {
                if (!level1Spawn.activeItem[i] || level1Spawn.activeItem[i].name != draggedObject.name) continue;
                level1Spawn.SpawnAnimal(i);
                break;
            }
        }
    }
}