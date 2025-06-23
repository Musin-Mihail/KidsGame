using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level1
{
    public class Level1Global : MonoBehaviour
    {
        public List<GameObject> AllAnimals = new();
        public List<GameObject> AllEmpty = new();
        public static List<GameObject> AllAnimalsStatic = new();
        public GameObject Finger;
        public static int WaitHint;
        private int _hintTime;
        private int _check;
        public static GameObject Level1Spawn;

        private void Awake()
        {
            WinBobbles.Victory = AllAnimals.Count;
            for (var i = 0; i < AllAnimals.Count; i++)
            {
                var chance = Random.Range(0, AllAnimals.Count - 1);
                var item = AllAnimals[i];
                AllAnimals[i] = AllAnimals[chance];
                AllAnimals[chance] = item;
            }

            AllAnimalsStatic = AllAnimals;
        }

        private void Start()
        {
            StartCoroutine(StartHint());
        }

        private IEnumerator StartHint()
        {
            while (WinBobbles.Victory != 0)
            {
                while (_hintTime < 4 && _check == 0)
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
            _check = 0;
            var itemName = "";

            foreach (var item in GetComponent<Level1Spawn>().SpawnPosition)
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
                foreach (var item in AllEmpty)
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
            Finger.transform.position = start;
            if (_check == 2)
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

            _check = 0;
            Finger.transform.position = new Vector3(0, 10, 0);
        }
    }
}