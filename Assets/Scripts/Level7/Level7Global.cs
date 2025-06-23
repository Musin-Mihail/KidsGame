using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level7
{
    public class Level7Global : MonoBehaviour
    {
        public List<GameObject> AllItem = new();
        public static List<GameObject> AllItemStatic = new();
        public GameObject Finger;
        public static int WaitHint;
        private int _hintTime;
        private int _stop;
        public static int NextFigure;
        private Vector3 _startPosition;
        private Vector3 _endPosition;

        private void Awake()
        {
            WinBobbles.Victory = 1;
            AllItemStatic = AllItem;
        }

        private void Start()
        {
            _endPosition = GetComponent<Level7Spawn>().TargetPosition[4].transform.position;
            StartCoroutine(StartHint());
            StartCoroutine(ChangeTasks());
        }

        private void Update()
        {
            if (WinBobbles.Victory == 0 && _stop == 0)
            {
                _stop = 1;
            }
        }

        private IEnumerator ChangeTasks()
        {
            for (var i = 0; i < 5; i++)
            {
                RandomItem();
                while (NextFigure != 1)
                {
                    yield return new WaitForSeconds(0.5f);
                }

                yield return new WaitForSeconds(2.0f);
                NextFigure = 0;
            }

            GetComponent<Level7Spawn>().DestroyAll();
            WinBobbles.Victory = 0;
        }

        private void RandomItem()
        {
            for (var i = 0; i < AllItem.Count; i++)
            {
                var chance = Random.Range(0, AllItem.Count - 1);
                var item = AllItem[i];
                AllItem[i] = AllItem[chance];
                AllItem[chance] = item;
            }

            GetComponent<Level7Spawn>().StartGame();
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
            if (WinBobbles.Victory > 0)
            {
                var targetName = GetComponent<Level7Spawn>().TargetPosition[4].name;
                foreach (var item in GetComponent<Level7Spawn>().SpawnPosition)
                {
                    if (item.name == targetName)
                    {
                        _startPosition = item.transform.position;
                        break;
                    }
                }

                _startPosition.z = -1;
                _endPosition.z = -1;
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