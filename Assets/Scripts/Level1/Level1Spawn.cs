using System.Collections.Generic;
using UnityEngine;

namespace Level1
{
    public class Level1Spawn : MonoBehaviour
    {
        public GameObject parent;
        public List<GameObject> startSpawnPositions = new();
        public List<GameObject> endSpawnPositions = new();

        [HideInInspector] public List<GameObject> activeItem;

        public void Initialization()
        {
            for (var i = 0; i < 3; i++)
            {
                SpawnAnimal(i);
            }
        }

        public void SpawnAnimal(int number)
        {
            if (Level1Global.instance.allAnimals.Count <= 0) return;
            var animal = Instantiate(Level1Global.instance.allAnimals[0], parent.transform, false);
            var moveItem = animal.GetComponent<MoveItem>();
            moveItem.Initialization(startSpawnPositions[number].transform.position, endSpawnPositions[number].transform.position);
            animal.name = Level1Global.instance.allAnimals[0].name;
            activeItem[number] = animal;
            Level1Global.instance.allAnimals.RemoveAt(0);
            StartCoroutine(moveItem.Move());
        }
    }
}