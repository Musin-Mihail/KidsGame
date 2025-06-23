using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level2
{
    public class Level2Global : MonoBehaviour
    {
        public List<GameObject> AllItem = new();
        public static List<GameObject> AllItemStatic = new();
        public List<GameObject> AllEmpty = new();
        public GameObject Finger;
        public GameObject Boat;
        public Vector3 TargetBoat;
        public static int WaitHint;
        private int _hintTime;
        private int _stop;

        private void Awake()
        {
            WinBobbles.Victory = AllItem.Count;
            for (var i = 0; i < AllItem.Count; i++)
            {
                var chance = Random.Range(0, AllItem.Count - 1);
                (AllItem[i], AllItem[chance]) = (AllItem[chance], AllItem[i]);
            }

            AllItemStatic = AllItem;
            TargetBoat = new Vector3(-15, 1.1f, 2.89f);
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
            while (Boat.transform.position != TargetBoat)
            {
                Boat.transform.position = Vector3.MoveTowards(Boat.transform.position, TargetBoat, 0.1f);
                yield return new WaitForSeconds(0.02f);
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
            var itemName = "";

            foreach (var item in GetComponent<Level2Spawn>().spawnPosition)
            {
                if (item.activeSelf)
                {
                    itemName = item.name;
                    start = item.transform.position;
                    start.z += -1;
                    check = 1;
                    break;
                }
            }

            if (check == 1)
            {
                foreach (var item in AllEmpty)
                {
                    if (itemName == item.name)
                    {
                        check = 2;
                        end = item.transform.position;
                        end.z += -1;
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