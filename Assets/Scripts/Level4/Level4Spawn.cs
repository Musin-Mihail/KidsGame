using System.Collections.Generic;
using UnityEngine;

namespace Level4
{
    public class Level4Spawn : MonoBehaviour
    {
        public List<GameObject> spawnPositionVector;
        [HideInInspector] public List<GameObject> spawnPosition = new() { null, null, null };

        private void Start()
        {
            for (var i = 0; i < 3; i++)
            {
                SpawnAnimal(i);
            }
        }

        private void SpawnAnimal(int number)
        {
            if (Level4Global.AllAnimalsStatic.Count <= 0) return;
            var animal = Instantiate(Level4Global.AllAnimalsStatic[0], spawnPositionVector[number].transform.position, Quaternion.identity);
            animal.name = Level4Global.AllAnimalsStatic[0].name;
            spawnPosition[number] = animal;
            Level4Global.AllAnimalsStatic.RemoveAt(0);
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