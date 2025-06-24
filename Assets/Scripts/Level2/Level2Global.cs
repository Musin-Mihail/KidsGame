using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level2
{
    public class Level2Global : MonoBehaviour
    {
        public static Level2Global Instance { get; private set; }

        public List<GameObject> AllItem = new();
        public List<GameObject> AllEmpty = new();
        public GameObject finger;
        public GameObject boat;
        public int waitHint;
        private Vector3 _targetBoat;
        private int _hintTime;
        private int _stop;

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
            WinBobbles.Instance.Victory = AllEmpty.Count;
            for (var i = 0; i < AllItem.Count; i++)
            {
                var chance = Random.Range(0, AllItem.Count - 1);
                (AllItem[i], AllItem[chance]) = (AllItem[chance], AllItem[i]);
            }

            _targetBoat = new Vector3(-15, 1.1f, 2.89f);
            StartCoroutine(StartHint());
        }

        private void Update()
        {
            if (WinBobbles.Instance.Victory == 0 && _stop == 0)
            {
                _stop = 1;
                StartCoroutine(Win2());
            }
        }

        private IEnumerator Win2()
        {
            while (boat.transform.position != _targetBoat)
            {
                boat.transform.position = Vector3.MoveTowards(boat.transform.position, _targetBoat, 0.1f);
                yield return new WaitForSeconds(0.02f);
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
            finger.transform.position = start;
            if (check == 2)
            {
                while (finger.transform.position != end)
                {
                    finger.transform.position = Vector3.MoveTowards(finger.transform.position, end, 0.1f);
                    if (waitHint == 1)
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