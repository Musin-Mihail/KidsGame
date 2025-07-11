using System.Collections.Generic;
using UnityEngine;

namespace Level6
{
    public class Level6Chest : MonoBehaviour
    {
        public List<Vector3> CollectedThings = new();
        public int BusyPlaces;

        private void Start()
        {
            CollectedThings.Add(new Vector3(-1.5f, 2, 1.2f));
            CollectedThings.Add(new Vector3(1.5f, 2, 1.1f));
            CollectedThings.Add(new Vector3(0, 2.5f, 1));
        }
    }
}