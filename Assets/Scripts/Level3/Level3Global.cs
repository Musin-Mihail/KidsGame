using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level3
{
    public class Level3Global : MonoBehaviour
    {
        public static Level3Global Instance { get; private set; }
        public List<GameObject> ThreeFigures = new();
        public List<GameObject> AllAnimals = new();
        public List<GameObject> AllItem = new();
        public GameObject Finger;
        public int waitHint;
        public int threeFiguresComplete;
        public GameObject Task;
        public GameObject Figure;
        public GameObject AnimalCenter;
        public int nextFigure;
        public int StageMove;
        private Vector3 _center;
        private Vector3 _endTarget;
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
        }

        private void Start()
        {
            threeFiguresComplete = -1;
            _center = new Vector3(0, 0, 3);
            _endTarget = new Vector3(15, 0, 3);
            WinBobbles.Instance.Victory = 1;
            for (var i = 0; i < AllAnimals.Count; i++)
            {
                var chance = Random.Range(0, AllAnimals.Count - 1);
                (AllAnimals[i], AllAnimals[chance]) = (AllAnimals[chance], AllAnimals[i]);
            }

            StartCoroutine(MoveAnimals());
            _endPosition = new Vector3(0, 0, 0);
            StartCoroutine(StartHint());
        }

        private void Update()
        {
            if (WinBobbles.Instance.Victory == 0 && _stop == 0)
            {
                _stop = 1;
            }

            if (nextFigure == 1)
            {
                nextFigure = 0;
                FigureChange();
            }
        }

        private IEnumerator MoveAnimals()
        {
            yield return new WaitForSeconds(1.0f);
            foreach (var item in AllAnimals)
            {
                waitHint = 1;
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

                waitHint = 1;
                while (item.transform.position != _endTarget)
                {
                    item.transform.position = Vector3.MoveTowards(item.transform.position, _endTarget, 0.1f);
                    yield return new WaitForSeconds(0.01f);
                }

                foreach (var item3 in ThreeFigures)
                {
                    item3.GetComponent<SpriteRenderer>().enabled = false;
                }

                threeFiguresComplete = -1;
                StageMove = 0;
            }

            WinBobbles.Instance.Victory = 0;
        }

        private void RandomItem()
        {
            for (var i = 0; i < AllItem.Count; i++)
            {
                var chance = Random.Range(0, AllItem.Count - 1);
                (AllItem[i], AllItem[chance]) = (AllItem[chance], AllItem[i]);
            }

            GetComponent<Level3Spawn>().StartGame();
        }

        private void FigureChange()
        {
            if (threeFiguresComplete >= 0)
            {
                ThreeFigures[threeFiguresComplete].GetComponent<SpriteRenderer>().enabled = true;
                ThreeFigures[threeFiguresComplete].GetComponent<SpriteRenderer>().sprite = Figure.GetComponent<SpriteRenderer>().sprite;
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
            while (WinBobbles.Instance.Victory != 0)
            {
                while (_hintTime < 4)
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
            if (WinBobbles.Instance.Victory > 0)
            {
                _startPosition.z = -1;
                _endPosition.z = -1;
                Finger.transform.position = _startPosition;
                while (Finger.transform.position != _endPosition)
                {
                    Finger.transform.position = Vector3.MoveTowards(Finger.transform.position, _endPosition, 0.1f);
                    if (waitHint == 1)
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