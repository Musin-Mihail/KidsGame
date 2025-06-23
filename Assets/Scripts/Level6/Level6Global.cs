using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level6
{
    public class Level6Global : MonoBehaviour
    {
        public List<GameObject> AllStars = new List<GameObject>();
        public static List<GameObject> AllCollectedStars = new List<GameObject>();
        public static List<GameObject> AllStarsStatic = new List<GameObject>();
        public List<GameObject> AllChest = new List<GameObject>();
        public GameObject Finger;
        public static int WaitHint = 0;
        public int WaitHintPublic;
        int HintTime = 0;
        int Stop = 0;
        public static GameObject _level6Spawn;

        void Awake()
        {
            WinBobbles.Victory = AllStars.Count;
            for (int i = 0; i < AllStars.Count; i++)
            {
                int chance = Random.Range(0, 11);
                var item = AllStars[i];
                AllStars[i] = AllStars[chance];
                AllStars[chance] = item;
            }

            AllStarsStatic = AllStars;
            AllCollectedStars = new List<GameObject>();
        }

        void Start()
        {
            StartCoroutine(StartHint());
        }

        void Update()
        {
            WaitHintPublic = WaitHint;
            if (WinBobbles.Victory == 0 && Stop == 0)
            {
                Stop = 1;
                StartCoroutine(Win2());
            }
        }

        IEnumerator Win2()
        {
            foreach (var item in AllCollectedStars)
            {
                StartCoroutine(item.GetComponent<WinUp>().Win());
                yield return new WaitForSeconds(0.05f);
            }
        }

        public IEnumerator StartHint()
        {
            while (true)
            {
                while (HintTime < 4)
                {
                    yield return new WaitForSeconds(1.0f);
                    if (WaitHint == 1)
                    {
                        HintTime = 0;
                        WaitHint = 0;
                        break;
                    }

                    HintTime++;
                }

                if (HintTime >= 4)
                {
                    StartCoroutine(Hint());
                }

                HintTime = 0;
                yield return new WaitForSeconds(1.0f);
            }
        }

        public IEnumerator Hint()
        {
            Vector3 Start = new Vector3(0, 10, 0);
            Vector3 End = new Vector3(0, 10, 0);
            int check = 0;
            string Tag = "";

            foreach (var item in GetComponent<Level6Spawn>().SpawnPosition)
            {
                if (item != null && item.transform.position == item.GetComponent<MoveItem>().StartPosition)
                {
                    Tag = item.tag;
                    Start = item.transform.position;
                    check = 1;
                    break;
                }
            }

            if (check == 1)
            {
                foreach (var item in AllChest)
                {
                    if (Start != null && Tag == item.tag)
                    {
                        End = item.transform.position;
                        break;
                    }
                }
            }

            Start.z = -1;
            Finger.transform.position = Start;
            if (WaitHint != 2)
            {
                while (Finger.transform.position != End)
                {
                    Finger.transform.position = Vector3.MoveTowards(Finger.transform.position, End, 0.1f);
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