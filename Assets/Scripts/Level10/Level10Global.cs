using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using System;

namespace Level10
{
    public class Level10Global : MonoBehaviour
    {
        public static Level10Global Instance { get; private set; }
        public List<GameObject> AllItem = new();
        public List<GameObject> AllTarget = new();
        public List<GameObject> AllSpawn = new();
        public List<GameObject> AllPlace;
        public static List<GameObject> AllBusyPlace = new();
        public List<float> AllScale;
        public List<string> AllSize;
        public static int next = 3;
        public GameObject Finger;
        public static int WaitHint;
        private int _hintTime;
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private IEnumerator _startHint;

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
            AllBusyPlace.Clear();
            _startHint = StartHint();
            next = 3;
            for (var i = 0; i < AllItem.Count; i++)
            {
                var chance = Random.Range(0, AllItem.Count - 1);
                (AllItem[i], AllItem[chance]) = (AllItem[chance], AllItem[i]);
            }

            WinBobbles.Instance.Victory = 1;
            AllPlace = new List<GameObject>();
            StartCoroutine(StartGame());
            StartCoroutine(_startHint);
        }

        private IEnumerator StartGame()
        {
            for (var item = 0; item < AllItem.Count; item++)
            {
                for (var i = 0; i < AllScale.Count; i++)
                {
                    var chance = Random.Range(0, AllScale.Count - 1);
                    var scale = AllScale[i];
                    var nameSize = AllSize[i];
                    AllScale[i] = AllScale[chance];
                    AllSize[i] = AllSize[chance];
                    AllScale[chance] = scale;
                    AllSize[chance] = nameSize;
                }

                for (int i = 0; i < 3; i++)
                {
                    var go = Instantiate(AllItem[item], AllSpawn[i].transform.position, Quaternion.identity);
                    go.name = AllItem[item].name;
                    AllPlace.Add(go);
                    go.transform.localScale = new Vector3(AllScale[i], AllScale[i], 1);
                    go.transform.tag = AllSize[i];
                }

                while (next != 0)
                {
                    yield return new WaitForSeconds(0.5f);
                }

                next = 3;
                yield return new WaitForSeconds(1.0f);
                AllPlace.Clear();
                WaitHint = 1;
            }

            StopCoroutine(_startHint);

            foreach (var item in AllBusyPlace)
            {
                StartCoroutine(item.GetComponent<WinUp>().Win());
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(0.5f);
            StartCoroutine(WinBobbles.Instance.Win());
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
            var scale = 0.0f;
            foreach (var item in AllPlace)
            {
                if (item.activeSelf)
                {
                    _startPosition = item.transform.position;
                    scale = item.transform.localScale.x;
                    break;
                }
            }

            foreach (var item in AllTarget)
            {
                if (item.transform.lossyScale.x == scale)
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