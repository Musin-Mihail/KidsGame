using System.Collections.Generic;
using UnityEngine;

namespace Level1
{
    public class Level1Spawn : MonoBehaviour
    {
        public List<GameObject> spawnPositionVector;
        public List<GameObject> spawnPosition;
        public Transform scale;
        public GameObject canvas;

        private void Start()
        {
            for (var i = 0; i < spawnPosition.Count; i++)
            {
                SpawnAnimal(i);
            }
        }

        public void SpawnAnimal(int number)
        {
            if (Level1Global.Instance.allAnimals.Count > 0)
            {
                var animal = Instantiate(Level1Global.Instance.allAnimals[0], spawnPositionVector[number].transform.position, Quaternion.identity);
                animal.transform.localScale = scale.transform.lossyScale;
                animal.name = Level1Global.Instance.allAnimals[0].name;
                spawnPosition[number] = animal;
                animal.transform.SetParent(canvas.transform);
                Level1Global.Instance.allAnimals.RemoveAt(0);
            }
        }
    }
}