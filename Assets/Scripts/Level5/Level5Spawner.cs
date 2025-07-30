using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Level5
{
    public class Level5Spawner : BaseSpawner
    {
        [Header("Настройки спаунера уровня 5")]
        public Transform targetsParent;
        public List<GameObject> spawnedTargets = new();

        public override void Initialization()
        {
            // В этом уровне инициализация управляется из Level5Manager
        }

        public void SpawnTargets(List<GameObject> targets)
        {
            for (var i = 0; i < startSpawnPositions.Count; i++)
            {
                if (i >= targets.Count) break;
                var targetPrefab = targets[i];
                var spawnPosition = startSpawnPositions[i].transform.position;
                var newTarget = Instantiate(targetPrefab, spawnPosition, targetPrefab.transform.rotation, targetsParent);
                newTarget.name = targetPrefab.name;
                spawnedTargets.Add(newTarget);
            }
        }
    }
}