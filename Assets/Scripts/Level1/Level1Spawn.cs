using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Level1
{
    public class Level1Spawn : BaseSpawner
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
            if (Level1Global.instance.allItems.Count <= 0) return;

            var animal = Instantiate(Level1Global.instance.allItems[0], parent.transform, false);
            animal.name = Level1Global.instance.allItems[0].name;

            var moveItem = animal.GetComponent<MoveItem>();
            if (moveItem)
            {
                moveItem.Initialization(startSpawnPositions[number].transform.position, endSpawnPositions[number].transform.position);
                StartCoroutine(moveItem.Move());
            }

            activeItem[number] = animal;
            Level1Global.instance.allItems.RemoveAt(0);
        }
    }
}