using System.Collections.Generic;
using UnityEngine;

namespace Level9
{
    public class Level9Spawn : MonoBehaviour
    {
        public List<GameObject> SpawnPositionVector = new();
        public List<GameObject> SpawnPosition = new();
        public Transform _scale;

        private void Awake()
        {
            Level9Global._level9Spawn = gameObject;
        }

        private void Start()
        {
            for (var i = 0; i < SpawnPosition.Count; i++)
            {
                if (Level9Global.AllItemStatic.Count > 0 && SpawnPosition[i] == null)
                {
                    SpawnItem(i);
                }
            }
        }

        private void SpawnItem(int number)
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
            for (var i = 0; i < 3; i++)
            {
                if (SpawnPosition[i].activeSelf == false)
                {
                    SpawnItem(i);
                }
            }
        }
    }
}