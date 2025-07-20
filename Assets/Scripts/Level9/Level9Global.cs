using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level9
{
    public class Level9Global : MonoBehaviour
    {
        public static Level9Global Instance { get; private set; }
        public List<GameObject> AllItem = new();
        public static List<GameObject> AllItemStatic = new();
        public List<GameObject> AllEmpty = new();
        public GameObject Finger;
        public static int WaitHint;
        public static GameObject _level9Spawn;

        private int _hintTime;
        private int _stop;
        private Vector3 _startPosition;
        private Vector3 _endPosition;

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

            for (var i = 0; i < AllItem.Count; i++)
            {
                var chance = Random.Range(0, AllItem.Count);
                (AllItem[i], AllItem[chance]) = (AllItem[chance], AllItem[i]);
            }

            AllItemStatic = AllItem;
            WinBobbles.instance.victory = AllItem.Count;
        }

        private void Start()
        {
            StartCoroutine(StartHint());
        }

        private void Update()
        {
            if (WinBobbles.instance.victory == 0 && _stop == 0)
            {
                _stop = 1;
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
            if (WinBobbles.instance.victory > 0)
            {
                var itemTag = " ";
                foreach (var item in GetComponent<Level9Spawn>().SpawnPosition)
                {
                    if (item.activeSelf)
                    {
                        _startPosition = item.transform.position;
                        itemTag = item.tag;
                        break;
                    }
                }

                foreach (var item in AllEmpty)
                {
                    if (item.tag == itemTag)
                    {
                        _endPosition = item.transform.position;
                        break;
                    }
                }

                Finger.transform.position = _startPosition;
                while (Finger.transform.position != _endPosition)
                {
                    Finger.transform.position = Vector3.MoveTowards(Finger.transform.position, _endPosition, 0.1f);
                    if (WaitHint == 1)
                    {
                        Finger.transform.position = new Vector3(0, 10, 0);
                        break;
                    }

                    yield return new WaitForSeconds(0.01f);
                }

                Finger.transform.position = new Vector3(0, 10, 0);
            }
        }
    }
}