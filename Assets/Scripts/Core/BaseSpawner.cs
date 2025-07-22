using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public abstract class BaseSpawner : MonoBehaviour
    {
        [Header("Настройки спаунера")]
        public Transform parent;
        public List<GameObject> startSpawnPositions;
        public List<GameObject> endSpawnPositions;
        [HideInInspector] public List<GameObject> activeItem;

        public abstract void Initialization();
    }
}