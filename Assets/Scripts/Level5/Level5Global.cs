using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level5
{
    public class Level5Global : MonoBehaviour
    {
        public List<GameObject> EmptyFigures;
        public List<GameObject> ColarFigures;
        public static List<GameObject> NewColarFigures;
        public List<GameObject> EmptyFiguresVector2;
        public static List<GameObject> ReadyFigures;
        public static List<GameObject> ReadyEmptyFigures;
        public List<Sprite> StageOyster;
        public static List<Sprite> NewStageOyster;
        public static GameObject Delete;
        public GameObject Chest;
        public GameObject OpenChest;
        public GameObject Finger;
        public static int WaitHint;
        public Transform _scale;
        private int _hintTime;
        private int _test;

        private void Awake()
        {
            ReadyEmptyFigures = new List<GameObject>();
            ReadyFigures = new List<GameObject>();
            WinBobbles.Victory = ColarFigures.Count;
            NewStageOyster = StageOyster;
            Delete = GameObject.Find("Delete");
        }

        private void Start()
        {
            for (var i = 0; i < ColarFigures.Count; i++)
            {
                var chance = Random.Range(0, 11);
                var item = ColarFigures[i];
                ColarFigures[i] = ColarFigures[chance];
                ColarFigures[chance] = item;
            }

            for (var i = 0; i < EmptyFigures.Count; i++)
            {
                var chance = Random.Range(0, 11);
                var item = EmptyFigures[i];
                EmptyFigures[i] = EmptyFigures[chance];
                EmptyFigures[chance] = item;
            }

            for (var i = 0; i < EmptyFiguresVector2.Count; i++)
            {
                var empty = Instantiate(EmptyFigures[i], EmptyFiguresVector2[i].transform.position, EmptyFigures[i].transform.rotation, Delete.transform);
                ReadyEmptyFigures.Add(empty);
                empty.transform.localScale = _scale.localScale;
            }

            NewColarFigures = ColarFigures;
            StartCoroutine(StartHint());
        }

        private void Update()
        {
            if (WinBobbles.Victory == 0 && _test == 0)
            {
                _test = 1;
                StartCoroutine(DestroyAll());
            }
        }

        private IEnumerator DestroyAll()
        {
            var allChildren = Delete.GetComponentsInChildren<Transform>();
            for (var i = 1; i < allChildren.Length; i++)
            {
                allChildren[i].GetComponent<SpriteRenderer>().enabled = false;
                yield return new WaitForSeconds(0.05f);
            }

            Chest.GetComponent<Animator>().enabled = true;
            yield return new WaitForSeconds(1.5f);
            Chest.SetActive(false);
            OpenChest.SetActive(true);
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

            foreach (var item in ReadyFigures)
            {
                if (item.activeSelf)
                {
                    itemTag = item.tag;
                    start = item.transform.position;
                    start.z = 0.0f;
                    check = 1;
                    break;
                }
            }

            if (check == 1)
            {
                foreach (var item in ReadyEmptyFigures)
                {
                    if (itemTag == item.tag)
                    {
                        end = item.transform.position;
                        end.z = 0.0f;
                        check = 2;
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