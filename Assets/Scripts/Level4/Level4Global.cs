using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level4
{
    public class Level4Global : MonoBehaviour
    {
        public List<GameObject> AllAnimals = new();
        public static List<GameObject> AllCollected = new();
        public static List<GameObject> AllAimalsStatic = new();
        public List<GameObject> AllZone = new();
        public GameObject Finger;
        public static int WaitHint;
        public static GameObject _level4Spawn;
        private int _hintTime;
        private int _stop;

        private void Awake()
        {
            WinBobbles.Victory = AllAnimals.Count;
            for (var i = 0; i < AllAnimals.Count; i++)
            {
                var chance = Random.Range(0, 9);
                var item = AllAnimals[i];
                AllAnimals[i] = AllAnimals[chance];
                AllAnimals[chance] = item;
            }

            AllAimalsStatic = AllAnimals;
            AllCollected = new List<GameObject>();
        }

        private void Start()
        {
            StartCoroutine(StartHint());
        }

        private void Update()
        {
            if (WinBobbles.Victory == 0 && _stop == 0)
            {
                _stop = 1;
                StartCoroutine(Win2());
            }
        }

        private IEnumerator Win2()
        {
            foreach (var item in AllCollected)
            {
                StartCoroutine(item.GetComponent<WinUp>().Win());
                yield return new WaitForSeconds(0.05f);
            }
        }

        private IEnumerator StartHint()
        {
            while (WinBobbles.Victory != 0)
            {
                while (_hintTime < 4)
                {
                    yield return new WaitForSeconds(1.0f);
                    if (WaitHint == 1)
                    {
                        _hintTime = 0;
                        WaitHint = 0;
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
            var check = 0;
            var itemTag = "";

            foreach (var item in GetComponent<Level4Spawn>().SpawnPosition)
            {
                if (item.activeSelf)
                {
                    itemTag = item.tag;
                    start = item.transform.position;
                    check = 1;
                    break;
                }
            }

            if (check == 1)
            {
                foreach (var item in AllZone)
                {
                    if (itemTag == item.tag)
                    {
                        check = 2;
                        end = item.transform.position;
                        break;
                    }
                }
            }

            start.z = -1;
            Finger.transform.position = start;
            if (check == 2)
            {
                while (Finger.transform.position != end)
                {
                    Finger.transform.position = Vector3.MoveTowards(Finger.transform.position, end, 0.1f);
                    if (WaitHint == 1)
                    {
                        Finger.transform.position = new Vector3(0, 10, 0);
                        break;
                    }

                    yield return new WaitForSeconds(0.01f);
                }
            }

            Finger.transform.position = new Vector3(0, 10, 0);
        }
    }
}