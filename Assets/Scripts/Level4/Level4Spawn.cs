using System.Collections.Generic;
using UnityEngine;

namespace Level4
{
    public class Level4Spawn : MonoBehaviour
    {
        public List<GameObject> spawnPositionVector;
        [HideInInspector] public List<GameObject> spawnPosition;

        private void Awake()
        {
            Level4Global._level4Spawn = gameObject;
        }

        private void Start()
        {
            for (var i = 0; i < 3; i++)
            {
                SpawnAnimal(i);
            }
        }

        private void SpawnAnimal(int number)
        {
            if (Level4Global.AllAimalsStatic.Count <= 0) return;
            var animal = Instantiate(Level4Global.AllAimalsStatic[0], spawnPositionVector[number].transform.position, Quaternion.identity);
            animal.name = Level4Global.AllAimalsStatic[0].name;
            spawnPosition[number] = animal;
            Level4Global.AllAimalsStatic.RemoveAt(0);
        }

        public void SearchFreeSpace(string nameAnimal)
        {
            for (var i = 0; i < 3; i++)
            {
                if (nameAnimal == spawnPosition[i].name)
                {
                    SpawnAnimal(i);
                }
            }
        }
    }
}