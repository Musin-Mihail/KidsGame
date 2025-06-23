using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level3
{
    public class Level3Global : MonoBehaviour
    {
        public List<GameObject> ThreeFigures = new();
        public List<GameObject> AllAnimals = new();
        public List<GameObject> AllItem = new();
        public static List<GameObject> AllItemStatic = new();
        public GameObject Finger;
        public static int WaitHint;
        public static int ThreeFiguresComplete;
        public GameObject Task;
        public GameObject Figure;
        public GameObject AnimalCenter;
        public static int NextFigure;
        public int StageMove;
        private Vector3 _center;
        private Vector3 _endTarget;
        private int _hintTime;
        private int _stop;
        private Vector3 _startPosition;
        private Vector3 _endPosition;

        private void Awake()
        {
            ThreeFiguresComplete = -1;
            _center = new Vector3(0, 0, 3);
            _endTarget = new Vector3(15, 0, 3);
            WinBobbles.Victory = 1;
            for (var i = 0; i < AllAnimals.Count; i++)
            {
                var chance = Random.Range(0, AllAnimals.Count - 1);
                var item = AllAnimals[i];
                AllAnimals[i] = AllAnimals[chance];
                AllAnimals[chance] = item;
            }

            AllItemStatic = AllItem;
            StartCoroutine(MoveAnimals());
        }

        private void Start()
        {
            _endPosition = new Vector3(0, 0, 0);
            StartCoroutine(StartHint());
        }

        private void Update()
        {
            if (WinBobbles.Victory == 0 && _stop == 0)
            {
                _stop = 1;
            }

            if (NextFigure == 1)
            {
                NextFigure = 0;
                FigureChange();
            }
        }

        private IEnumerator MoveAnimals()
        {
            yield return new WaitForSeconds(1.0f);
            foreach (var item in AllAnimals)
            {
                WaitHint = 1;
                RandomItem();
                while (item.transform.position != _center)
                {
                    item.transform.position = Vector3.MoveTowards(item.transform.position, _center, 0.1f);
                    yield return new WaitForSeconds(0.01f);
                }

                AnimalCenter = item;
                FigureChange();
                Figure.GetComponent<SpriteRenderer>().enabled = true;
                Task.GetComponent<SpriteRenderer>().enabled = true;
                while (StageMove != 1)
                {
                    yield return new WaitForSeconds(1);
                }

                Figure.GetComponent<SpriteRenderer>().enabled = false;
                Task.GetComponent<SpriteRenderer>().enabled = false;
                foreach (var item2 in GetComponent<Level3Spawn>().SpawnPosition)
                {
                    item2.SetActive(false);
                }

                WaitHint = 1;
                while (item.transform.position != _endTarget)
                {
                    item.transform.position = Vector3.MoveTowards(item.transform.position, _endTarget, 0.1f);
                    yield return new WaitForSeconds(0.01f);
                }

                foreach (var item3 in ThreeFigures)
                {
                    item3.GetComponent<SpriteRenderer>().enabled = false;
                }

                ThreeFiguresComplete = -1;
                StageMove = 0;
            }

            WinBobbles.Victory = 0;
        }

        private void RandomItem()
        {
            for (var i = 0; i < AllItemStatic.Count; i++)
            {
                var chance = Random.Range(0, AllItemStatic.Count - 1);
                var item = AllItemStatic[i];
                AllItemStatic[i] = AllItemStatic[chance];
                AllItemStatic[chance] = item;
            }

            GetComponent<Level3Spawn>().StartGame();
        }

        private void FigureChange()
        {
            if (ThreeFiguresComplete >= 0)
            {
                ThreeFigures[ThreeFiguresComplete].GetComponent<SpriteRenderer>().enabled = true;
                ThreeFigures[ThreeFiguresComplete].GetComponent<SpriteRenderer>().sprite = Figure.GetComponent<SpriteRenderer>().sprite;
            }

            if (GetComponent<Level3Spawn>().SpawnPosition.Count > 2)
            {
                var NewRandom = Random.Range(0, GetComponent<Level3Spawn>().SpawnPosition.Count);
                if (GetComponent<Level3Spawn>().SpawnPosition[NewRandom] == null)
                {
                    FigureChange();
                }
                else
                {
                    Figure.GetComponent<SpriteRenderer>().sprite = GetComponent<Level3Spawn>().SpawnPosition[NewRandom].GetComponent<SpriteRenderer>().sprite;
                    AnimalCenter.name = GetComponent<Level3Spawn>().SpawnPosition[NewRandom].name;
                    _startPosition = GetComponent<Level3Spawn>().SpawnPosition[NewRandom].transform.position;
                    GetComponent<Level3Spawn>().SpawnPosition.RemoveAt(NewRandom);
                }
            }
            else
            {
                StageMove = 1;
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
            if (WinBobbles.Victory > 0)
            {
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