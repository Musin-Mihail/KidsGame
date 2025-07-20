using System.Collections.Generic;
using UnityEngine;

namespace Level2
{
    public class Level2Spawn : MonoBehaviour
    {
        public Transform parent;
        public List<GameObject> startSpawnPositions;
        public List<GameObject> endSpawnPositions;
        [HideInInspector] public List<GameObject> activeItem;

        public void Initialization()
        {
            SpawnAnimal();
        }

        private void SpawnAnimal()
        {
            for (var i = 0; i < startSpawnPositions.Count; i++)
            {
                var newItem = Instantiate(Level2Global.instance.allItem[i], parent, false);
                newItem.name = Level2Global.instance.allItem[i].name;

                var moveItem = newItem.GetComponent<MoveItem>();
                moveItem.Initialization(startSpawnPositions[i].transform.position, endSpawnPositions[i].transform.position);
                StartCoroutine(moveItem.Move());

                activeItem.Add(newItem);
            }
        }
    }
}