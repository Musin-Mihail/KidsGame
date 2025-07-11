using System.Collections.Generic;
using UnityEngine;

namespace Level3
{
    public class Level3Spawn : MonoBehaviour
    {
        public List<GameObject> SpawnPositionVector;
        public List<GameObject> SpawnPosition;

        public void StartGame()
        {
            SpawnPosition = new List<GameObject>();
            for (var i = 0; i < 5; i++)
            {
                var animal = Instantiate(Level3Global.Instance.AllItem[i], SpawnPositionVector[i].transform.position, Quaternion.identity);
                animal.name = Level3Global.Instance.AllItem[i].name;
                SpawnPosition.Add(animal);
            }
        }
    }
}