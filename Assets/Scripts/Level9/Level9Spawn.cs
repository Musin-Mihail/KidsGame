using System.Collections.Generic;
using UnityEngine;

namespace Level9
{
    public class Level9Spawn : MonoBehaviour
    {
        public List<GameObject> SpawnPositionVector = new List<GameObject>(); // Места спавна
        public List<GameObject> SpawnPosition = new List<GameObject>(); // Появившиеся предметы на сцене
        public static int Next = 1;
        public Transform _scale;

        void Awake()
        {
            Level9Global._level9Spawn = gameObject;
        }

        void Start()
        {
            for (int i = 0; i < SpawnPosition.Count; i++)
            {
                if (Level9Global.AllItemStatic.Count > 0 && SpawnPosition[i] == null)
                {
                    SpawnItem(i);
                }
            }
        }

        void SpawnItem(int number)
        {
            if (Level9Global.AllItemStatic.Count > 0)
            {
                var animal = Instantiate(Level9Global.AllItemStatic[0], SpawnPositionVector[number].transform.position, Quaternion.identity);
                animal.transform.localScale = _scale.lossyScale;
                SpawnPosition[number] = animal;
                Level9Global.AllItemStatic.RemoveAt(0);
            }
        }

        public void SearchFreePlace()
        {
            for (int i = 0; i < 3; i++)
            {
                if (SpawnPosition[i].activeSelf == false)
                {
                    SpawnItem(i);
                }
            }
        }
    }
}