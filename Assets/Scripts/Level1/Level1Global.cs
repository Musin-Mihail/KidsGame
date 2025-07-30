using Core;
using UnityEngine;

namespace Level1
{
    public class Level1Global : BaseLevelManager<Level1Global>
    {
        [HideInInspector] public Level1Spawn level1Spawn;

        protected override void Awake()
        {
            base.Awake();
            level1Spawn = GetComponent<Level1Spawn>();
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
    }
}