using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level4
{
    public class Level4Global : MonoBehaviour
    {
        public static Level4Global instance { get; private set; }
        public List<GameObject> allAnimals = new();
        public static List<GameObject> AllCollected = new();
        public static List<GameObject> AllAnimalsStatic = new();
        public List<GameObject> allZone = new();
        public GameObject finger;
        public static int WaitHint;
        public Level4Spawn level4Spawn;
        private int _hintTime;
        private int _stop;

        private void Awake()
        {
            if (instance && !Equals(instance, this))
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        private void Start()
        {
            WinBobbles.instance.victory = allAnimals.Count;
            for (var i = 0; i < allAnimals.Count; i++)
            {
                var chance = Random.Range(0, 9);
                (allAnimals[i], allAnimals[chance]) = (allAnimals[chance], allAnimals[i]);
            }

            AllAnimalsStatic = allAnimals;
            AllCollected = new List<GameObject>();
            StartCoroutine(StartHint());
        }

        private void Update()
        {
            if (WinBobbles.instance.victory == 0 && _stop == 0)
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
            while (WinBobbles.instance.victory != 0)
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

            foreach (var item in GetComponent<Level4Spawn>().spawnPosition)
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
                foreach (var item in allZone)
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
            finger.transform.position = start;
            if (check == 2)
            {
                while (finger.transform.position != end)
                {
                    finger.transform.position = Vector3.MoveTowards(finger.transform.position, end, 0.1f);
                    if (WaitHint == 1)
                    {
                        finger.transform.position = new Vector3(0, 10, 0);
                        break;
                    }

                    yield return new WaitForSeconds(0.01f);
                }
            }

            finger.transform.position = new Vector3(0, 10, 0);
        }
    }
}