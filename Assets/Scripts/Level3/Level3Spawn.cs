using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Level3
{
    public class Level3Spawn : BaseSpawner
    {
        public override void Initialization()
        {
            // Логика инициализации управляется из Level3Global
        }

        public void SpawnAnimal()
        {
            foreach (var oldItem in activeItem.Where(oldItem => oldItem))
            {
                Destroy(oldItem);
            }

            activeItem = new List<GameObject>();
            for (var i = 0; i < 5; i++)
            {
                if (i >= Level3Global.instance.allItems.Count) break;
                var item = Instantiate(Level3Global.instance.allItems[i], startSpawnPositions[i].transform.position, Quaternion.identity);
                item.name = Level3Global.instance.allItems[i].name;
                var moveItem = item.GetComponent<MoveItem>();
                if (moveItem)
                {
                    moveItem.Initialization(startSpawnPositions[i].transform.position, endSpawnPositions[i].transform.position);
                    StartCoroutine(moveItem.Move());
                }

                activeItem.Add(item);
            }
        }
    }
}