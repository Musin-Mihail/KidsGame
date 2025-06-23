using System.Collections.Generic;
using UnityEngine;

namespace Level7
{
    public class Level7Spawn : MonoBehaviour
    {
        public List<GameObject> SpawnPositionVector = new();
        public List<GameObject> SpawnPosition = new();
        public List<GameObject> TargetPosition = new();
        private Sprite _circle;
        private Vector3 _startScale;

        private void Start()
        {
            _circle = TargetPosition[4].GetComponent<SpriteRenderer>().sprite;
            _startScale = TargetPosition[4].transform.localScale;
        }

        public void StartGame()
        {
            TargetPosition[4].GetComponent<SpriteRenderer>().sprite = _circle;
            TargetPosition[4].transform.localScale = _startScale;
            for (var i = 0; i < SpawnPosition.Count; i++)
            {
                if (SpawnPosition[i])
                {
                    Destroy(SpawnPosition[i].gameObject);
                }

                var animal = Instantiate(Level7Global.AllItemStatic[i], SpawnPositionVector[i].transform.position, Quaternion.identity);
                animal.name = Level7Global.AllItemStatic[i].name;
                SpawnPosition[i] = animal;
            }

            for (var i = 0; i < SpawnPosition.Count; i++)
            {
                var chance = Random.Range(0, SpawnPosition.Count - 1);
                var item = SpawnPosition[i];
                SpawnPosition[i] = SpawnPosition[chance];
                SpawnPosition[chance] = item;
            }

            TargetPosition[0].GetComponent<SpriteRenderer>().sprite = SpawnPosition[1].GetComponent<SpriteRenderer>().sprite;
            TargetPosition[0].transform.localScale = SpawnPosition[1].transform.localScale * 100;
            TargetPosition[1].GetComponent<SpriteRenderer>().sprite = SpawnPosition[2].GetComponent<SpriteRenderer>().sprite;
            TargetPosition[1].transform.localScale = SpawnPosition[2].transform.localScale * 100;
            TargetPosition[2].GetComponent<SpriteRenderer>().sprite = SpawnPosition[1].GetComponent<SpriteRenderer>().sprite;
            TargetPosition[2].transform.localScale = SpawnPosition[1].transform.localScale * 100;
            TargetPosition[3].GetComponent<SpriteRenderer>().sprite = SpawnPosition[2].GetComponent<SpriteRenderer>().sprite;
            TargetPosition[3].transform.localScale = SpawnPosition[2].transform.localScale * 100;
            TargetPosition[4].name = SpawnPosition[1].name;
            Level7Global.WaitHint = 1;
        }

        public void DestroyAll()
        {
            for (var i = 0; i < SpawnPosition.Count; i++)
            {
                SpawnPosition[i].gameObject.SetActive(false);
            }
        }
    }
}