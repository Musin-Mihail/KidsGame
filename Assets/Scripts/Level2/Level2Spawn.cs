using System.Collections.Generic;
using UnityEngine;

namespace Level2
{
    public class Level2Spawn : MonoBehaviour
    {
        public List<Transform> spawnPositionVector;
        public List<GameObject> spawnPosition;
        public Transform scale;
        public Transform scaleWindows3;
        public Transform scaleWindows1;

        private void Start()
        {
            spawnPosition = new List<GameObject>();
            foreach (var item in spawnPositionVector)
            {
                var newItem = Instantiate(Level2Global.Instance.AllItem[0], item.position, Quaternion.identity);
                newItem.name = Level2Global.Instance.AllItem[0].name;
                if (newItem.name == "Window3")
                {
                    newItem.transform.localScale = scaleWindows3.transform.lossyScale * 1.5f;
                }
                else if (newItem.name == "Window1")
                {
                    newItem.transform.localScale = scaleWindows1.transform.lossyScale * 1.5f;
                }
                else
                {
                    newItem.transform.localScale = scale.transform.lossyScale * 1.5f;
                }

                spawnPosition.Add(newItem);
                Level2Global.Instance.AllItem.RemoveAt(0);
            }
        }
    }
}