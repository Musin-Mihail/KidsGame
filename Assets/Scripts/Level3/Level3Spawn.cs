using System.Collections.Generic;
using UnityEngine;

namespace Level3
{
    public class Level3Spawn : MonoBehaviour
    {
        public List<GameObject> startSpawnPositions;
        public List<GameObject> endSpawnPositions;
        public List<GameObject> activeItem;

        public void SpawnAnimal()
        {
            activeItem = new List<GameObject>();
            for (var i = 0; i < 5; i++)
            {
                var item = Instantiate(Level3Global.instance.allItem[i], startSpawnPositions[i].transform.position, Quaternion.identity);
                item.name = Level3Global.instance.allItem[i].name;
                var moveItem = item.GetComponent<MoveItem>();
                moveItem.Initialization(startSpawnPositions[i].transform.position, endSpawnPositions[i].transform.position);
                StartCoroutine(moveItem.Move());
                activeItem.Add(item);
            }
        }
    }
}