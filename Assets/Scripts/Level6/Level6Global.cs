using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level6
{
    public class Level6Global : MonoBehaviour
    {
        public List<GameObject> AllStars = new();
        public static List<GameObject> AllCollectedStars = new();
        public static List<GameObject> AllStarsStatic = new();
        public List<GameObject> AllChest = new();
        public GameObject Finger;
        public static int WaitHint;
        public int WaitHintPublic;
        public static GameObject _level6Spawn;
        private int _hintTime = 0;
        private int _stop = 0;

        private void Awake()
        {
            WinBobbles.Victory = AllStars.Count;
            for (var i = 0; i < AllStars.Count; i++)
            {
                var chance = Random.Range(0, 11);
                var item = AllStars[i];
                AllStars[i] = AllStars[chance];
                AllStars[chance] = item;
            }

            AllStarsStatic = AllStars;
            AllCollectedStars = new List<GameObject>();
        }

        void Start()
        {
            StartCoroutine(StartHint());
        }

        private void Update()
        {
            WaitHintPublic = WaitHint;
            if (WinBobbles.Victory == 0 && _stop == 0)
            {
                _stop = 1;
                StartCoroutine(Win2());
            }
        }

        private IEnumerator Win2()
        {
            foreach (var item in AllCollectedStars)
            {
                StartCoroutine(item.GetComponent<WinUp>().Win());
                yield return new WaitForSeconds(0.05f);
            }
        }

        private IEnumerator StartHint()
        {
            while (true)
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

            foreach (var item in GetComponent<Level6Spawn>().SpawnPosition)
            {
                if (item && item.transform.position == item.GetComponent<MoveItem>().StartPosition)
                {
                    itemTag = item.tag;
                    start = item.transform.position;
                    check = 1;
                    break;
                }
            }

            if (check == 1)
            {
                foreach (var item in AllChest)
                {
                    if (itemTag == item.tag)
                    {
                        end = item.transform.position;
                        break;
                    }
                }
            }

            start.z = -1;
            Finger.transform.position = start;
            if (WaitHint != 2)
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