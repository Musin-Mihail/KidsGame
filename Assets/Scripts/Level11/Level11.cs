using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Level11
{
    public class Level11 : MonoBehaviour
    {
        public static Level11 Instance { get; private set; }
        public List<GameObject> AllItem = new();
        public List<GameObject> AllSpawn = new();
        public List<GameObject> AllTarget = new();
        public List<GameObject> AllFishChest = new();
        public static List<GameObject> AllFishChestStatic = new();
        public static List<GameObject> AllTargetStatic = new();
        public static List<GameObject> Delete = new();
        public GameObject EmptyChest;
        public GameObject FishChest;
        public GameObject TargetDistans;
        public static int count;
        public static int WaitHint;
        public GameObject Finger;
        public static Transform _scaleStatic;
        public Transform _scale;
        private int _hintTime;
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
            _scaleStatic = _scale;
            AllFishChestStatic.Clear();
            count = 0;
            AllTargetStatic = AllTarget;
            WinBobbles.instance.victory = 8;
            for (var i = 0; i < 8; i++)
            {
                AllItem.Add(FishChest);
            }

            for (var i = 0; i < 24; i++)
            {
                AllItem.Add(EmptyChest);
            }

            for (var i = 0; i < AllItem.Count; i++)
            {
                var chance = Random.Range(0, AllItem.Count - 1);
                (AllItem[i], AllItem[chance]) = (AllItem[chance], AllItem[i]);
            }

            AllSpawn = AllSpawn.OrderBy(x => Vector2.Distance(TargetDistans.transform.position, x.transform.position)).ToList();
            StartCoroutine(StartGame());
            StartCoroutine(StartHint());
        }

        private IEnumerator StartGame()
        {
            yield return new WaitForSeconds(1.0f);
            for (var i = 0; i < AllSpawn.Count; i++)
            {
                var chest = Instantiate(AllItem[i], AllSpawn[i].transform.position, Quaternion.identity);
                chest.name = AllItem[i].name;
                chest.transform.localScale = _scale.lossyScale;
                if (chest.name == "EmptyChest")
                {
                    Delete.Add(chest);
                }
                else
                {
                    AllFishChest.Add(chest);
                }

                yield return new WaitForSeconds(0.01f);
            }
        }

        private IEnumerator StartHint()
        {
            yield return new WaitForSeconds(4.0f);
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
                var newlist = AllFishChest.Where(x => x.name == "FishChest").OrderBy(x => Vector3.Distance(Finger.transform.position, x.transform.position)).ToList();
                _endPosition = newlist[0].transform.position;
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