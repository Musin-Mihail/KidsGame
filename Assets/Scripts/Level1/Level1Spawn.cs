using System.Collections.Generic;
using UnityEngine;

namespace Level1
{
    public class Level1Spawn : MonoBehaviour
    {
        public List<GameObject> SpawnPositionVector;
        public List<GameObject> SpawnPosition;
        public Transform _scale;

        private void Awake()
        {
            Level1Global.Level1Spawn = gameObject;
        }

        private void Start()
        {
            for (var i = 0; i < SpawnPosition.Count; i++)
            {
                SpawnAnimal(i);
            }
        }

        public void SpawnAnimal(int number)
        {
            if (Level1Global.AllAnimalsStatic.Count > 0)
            {
                var animal = Instantiate(Level1Global.AllAnimalsStatic[0], SpawnPositionVector[number].transform.position, Quaternion.identity);
                animal.transform.localScale = _scale.transform.lossyScale;
                animal.name = Level1Global.AllAnimalsStatic[0].name;
                SpawnPosition[number] = animal;
                Level1Global.AllAnimalsStatic.RemoveAt(0);
            }
        }
    }
}