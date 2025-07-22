using System.Collections.Generic;
using UnityEngine;

namespace Level3
{
    public class Level3Spawn : MonoBehaviour
    {
        public List<GameObject> spawnPositionVector;
        [HideInInspector] public List<GameObject> activeItem;


        public void SpawnAnimal()
        {
            activeItem = new List<GameObject>();
            for (var i = 0; i < 5; i++)
            {
                var item = Instantiate(Level3Global.instance.allItem[i], spawnPositionVector[i].transform.position, Quaternion.identity);
                item.name = Level3Global.instance.allItem[i].name;
                activeItem.Add(item);
            }
        }
    }
}