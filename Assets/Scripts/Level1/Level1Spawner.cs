using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Level1
{
    public class Level1Spawner : BaseSpawner
    {
        public override void Initialization()
        {
            activeItem = new List<GameObject>(new GameObject[startSpawnPositions.Count]);
            for (var i = 0; i < 3; i++)
            {
                SpawnAnimal(i);
            }
        }

        public void SpawnAnimal(int number)
        {
            if (Level1Manager.instance.allItems.Count <= 0) return;
            var animal = Instantiate(Level1Manager.instance.allItems[0], parent, false);
            animal.name = Level1Manager.instance.allItems[0].name;
            var moveItem = animal.GetComponent<MoveItem>();
            if (moveItem)
            {
                moveItem.Initialization(startSpawnPositions[number].transform.position, endSpawnPositions[number].transform.position);
                StartCoroutine(moveItem.Move());
            }

            activeItem[number] = animal;
            Level1Manager.instance.allItems.RemoveAt(0);
        }
    }
}