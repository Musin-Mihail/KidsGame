using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level8
{
    public class Level8 : MonoBehaviour
    {
        public static Level8 Instance { get; private set; }
        public List<GameObject> allItem = new();
        public List<GameObject> allPlace = new();
        public List<GameObject> allSpawn = new();
        public int end;
        public int countItem;
        public Sprite baseSprite;
        public GameObject nextAnimal;
        private int _hintTime;
        public GameObject finger;
        public int waitHint;
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
            _startHint = StartHint();
            WinBobbles.instance.victory = 1;
            StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            var center = new Vector3(0, 0, 0);
            var endVector = new Vector3(-18, 0, 0);
            GetComponent<Animator>().enabled = true;
            while (transform.position != center)
            {
                transform.position = Vector3.MoveTowards(transform.position, center, 0.1f);
                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(0.5f);
            GetComponent<Animator>().enabled = false;
            foreach (var item in allItem)
            {
                item.GetComponent<SpriteRenderer>().enabled = true;
            }

            GetComponent<SpriteRenderer>().sprite = baseSprite;
            yield return new WaitForSeconds(0.5f);
            GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 120);
            for (var i = 0; i < allItem.Count; i++)
            {
                StartCoroutine(allItem[i].GetComponent<Level8MoveItem>().Move(i));
            }

            StartCoroutine(_startHint);
            while (end != 1)
            {
                yield return new WaitForSeconds(0.5f);
            }

            foreach (var item in allItem)
            {
                item.GetComponent<SpriteRenderer>().enabled = false;
            }

            GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
            GetComponent<Animator>().enabled = true;
            StopCoroutine(_startHint);
            while (transform.position != endVector)
            {
                transform.position = Vector3.MoveTowards(transform.position, endVector, 0.1f);
                yield return new WaitForSeconds(0.01f);
            }

            if (nextAnimal)
            {
                nextAnimal.SetActive(true);
            }
            else
            {
                StartCoroutine(WinBobbles.instance.Win());
            }
        }

        private IEnumerator StartHint()
        {
            while (true)
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
            var newName = "";
            if (WinBobbles.instance.victory > 0)
            {
                foreach (var item in allItem)
                {
                    if (item.GetComponent<BoxCollider2D>().enabled == true)
                    {
                        _startPosition = item.transform.position;
                        newName = item.name;
                        break;
                    }
                }

                foreach (var item in allPlace)
                {
                    if (item.name == newName)
                    {
                        _endPosition = item.transform.position;
                        break;
                    }
                }

                finger.transform.position = _startPosition;
                while (finger.transform.position != _endPosition)
                {
                    finger.transform.position = Vector3.MoveTowards(finger.transform.position, _endPosition, 0.1f);
                    if (waitHint == 1)
                    {
                        finger.transform.position = new Vector3(0, 10, 0);
                        break;
                    }

                    yield return new WaitForSeconds(0.01f);
                }

                finger.transform.position = new Vector3(0, 10, 0);
            }
        }
    }
}