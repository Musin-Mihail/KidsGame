using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Level10
{
    public class Level10Spawner : BaseSpawner
    {
        [HideInInspector] public GameObject itemToSpawn;
        [HideInInspector] public List<float> scales;
        [HideInInspector] public List<string> sizes;

        /// <summary>
        /// Инициализирует и спаунит предметы для текущего раунда.
        /// </summary>
        public override void Initialization()
        {
            foreach (var item in activeItem.Where(item => item))
            {
                Destroy(item);
            }

            activeItem.Clear();
            if (!itemToSpawn || scales == null || sizes == null || scales.Count == 0 || sizes.Count == 0)
            {
                Debug.LogError("Данные для спауна (itemToSpawn, scales, sizes) не установлены!");
                return;
            }

            for (var i = 0; i < startSpawnPositions.Count; i++)
            {
                if (i >= scales.Count || i >= sizes.Count)
                {
                    Debug.LogWarning($"Недостаточно данных в scales/sizes для спауна на позиции {i}");
                    break;
                }

                var go = Instantiate(itemToSpawn, startSpawnPositions[i].transform.position, Quaternion.identity, parent);
                go.name = itemToSpawn.name;
                go.transform.localScale = new Vector3(scales[i], scales[i], 1);
                go.tag = sizes[i];
                if (go.TryGetComponent<MoveItem>(out var moveItem))
                {
                    moveItem.enabled = false;
                }

                activeItem.Add(go);
            }
        }
    }
}