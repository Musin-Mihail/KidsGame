using System.Collections.Generic;
using UnityEngine;

namespace Level6
{
    public class Level6Spawn : MonoBehaviour
    {
        public List<GameObject> SpawnPositionVector;
        public List<GameObject> SpawnPosition;

        void Awake()
        {
            Level6Global._level6Spawn = gameObject;
        }

        void Start()
        {
            for (int i = 0; i < SpawnPosition.Count; i++)
            {
                SpawnStars(i);
            }
        }

        void SpawnStars(int number)
        {
            if (Level6Global.AllStarsStatic.Count > 0)
            {
                var star = Instantiate(Level6Global.AllStarsStatic[0], SpawnPositionVector[number].transform.position, Quaternion.identity);
                SpawnPosition[number] = star;
                Level6Global.AllStarsStatic.RemoveAt(0);
            }
        }

        public void SearchFreeSpace()
        {
            for (int i = 0; i < 5; i++)
            {
                if (SpawnPosition[i].activeSelf == false)
                {
                    SpawnStars(i);
                    break;
                }
            }
        }
    }
}