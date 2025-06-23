using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level12
{
    public class Level12 : MonoBehaviour
    {
        public List<GameObject> AllTarget = new();
        public static List<GameObject> AllTargetStatic = new();
        public List<GameObject> AllItem = new();
        public static List<GameObject> AllItemStatic = new();
        public static int count;
        public GameObject Target;
        public static int WaitHint;
        public GameObject Finger;
        private int _hintTime;
        private Vector3 _endPosition;

        private void Start()
        {
            AllItemStatic = AllItem;
            count = 0;
            WinBobbles.Victory = 8;
            for (var i = 0; i < AllTarget.Count; i++)
            {
                var chance = Random.Range(0, AllTarget.Count - 1);
                var item = AllTarget[i];
                AllTarget[i] = AllTarget[chance];
                AllTarget[chance] = item;
            }

            Invoke("StartGame", 5.5f);
            StartCoroutine(StartHint());
        }

        private void StartGame()
        {
            Target.GetComponent<Animator>().enabled = false;
            AllTargetStatic = AllTarget;
            AllTargetStatic[0].GetComponent<Animator>().Play("Scale");
        }

        public static void NextFigure()
        {
            AllTargetStatic[count].GetComponent<Animator>().Play("Empty");
            count++;
            if (count <= 7)
            {
                AllTargetStatic[count].GetComponent<Animator>().Play("Scale");
            }
        }

        private IEnumerator StartHint()
        {
            yield return new WaitForSeconds(5.5f);
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
                foreach (var item in AllItem)
                {
                    if (item.name == AllTargetStatic[count].name)
                    {
                        _endPosition = item.transform.position;
                        break;
                    }
                }

                while (Finger.transform.position != _endPosition)
                {
                    Finger.transform.position = Vector3.MoveTowards(Finger.transform.position, _endPosition, 0.1f);
                    if (WaitHint == 1)
                    {
                        Finger.transform.position = new Vector3(0, -6, 0);
                        break;
                    }

                    yield return new WaitForSeconds(0.01f);
                }

                Finger.transform.position = new Vector3(0, -6, 0);
            }
        }
    }
}