using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level1
{
    public class Level1Global : MonoBehaviour
    {
        public static Level1Global Instance { get; private set; }
        public List<GameObject> allAnimals = new();
        public List<GameObject> allEmpty = new();
        public GameObject finger;
        public int waitHint;
        public Level1Spawn level1Spawn;
        private int _hintTime;
        private int _check;

        private void Awake()
        {
            if (Instance && !Equals(Instance, this))
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            WinBobbles.Instance.Victory = allEmpty.Count;
            for (var i = 0; i < allAnimals.Count; i++)
            {
                var chance = Random.Range(0, allAnimals.Count - 1);
                (allAnimals[i], allAnimals[chance]) = (allAnimals[chance], allAnimals[i]);
            }

            StartCoroutine(StartHint());
        }

        private IEnumerator StartHint()
        {
            while (WinBobbles.Instance.Victory != 0)
            {
                while (_hintTime < 4 && _check == 0)
                {
                    yield return new WaitForSeconds(1.0f);
                    if (waitHint == 1)
                    {
                        _hintTime = 0;
                        waitHint = 0;
                        break;
                    }

                    _hintTime++;
                }

                if (_hintTime >= 4)
                {
                    StartCoroutine(Hint());
                }

                _hintTime = 0;
                yield return new WaitForSeconds(1.0f);
            }
        }

        private IEnumerator Hint()
        {
            var start = new Vector3(0, 10, 0);
            var end = new Vector3(0, 10, 0);
            _check = 0;
            var itemName = "";
            if (!level1Spawn) yield break;
            foreach (var item in level1Spawn.GetComponent<Level1Spawn>().spawnPosition)
            {
                if (item.activeSelf)
                {
                    itemName = item.name;
                    start = item.transform.position;
                    _check = 1;
                    break;
                }
            }

            if (_check == 1)
            {
                foreach (var item in allEmpty)
                {
                    if (itemName == item.name)
                    {
                        _check = 2;
                        end = item.transform.position;
                        break;
                    }
                }
            }

            start.z = -1;
            finger.transform.position = start;
            if (_check == 2)
            {
                while (finger.transform.position != end)
                {
                    finger.transform.position = Vector3.MoveTowards(finger.transform.position, end, 0.1f);
                    if (waitHint == 1)
                    {
                        finger.transform.position = new Vector3(0, 10, 0);
                        break;
                    }

                    yield return new WaitForSeconds(0.01f);
                }
            }

            _check = 0;
            finger.transform.position = new Vector3(0, 10, 0);
        }
    }
}