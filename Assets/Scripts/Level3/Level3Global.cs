using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Level3
{
    public class Level3Global : MonoBehaviour
    {
        public static Level3Global instance { get; private set; }
        public Hint hint;
        public List<GameObject> threeFigures = new();
        public List<GameObject> allAnimals = new();
        public List<GameObject> allItem = new();
        public GameObject task;
        public GameObject figure;

        [HideInInspector] public GameObject animalCenter;
        [HideInInspector] public int nextFigure;
        [HideInInspector] public int stageMove;
        [HideInInspector] public int threeFiguresComplete;

        private Vector3 _center;
        private Vector3 _endTarget;
        private int _stop;
        private Level3Spawn _level3Spawn;
        private string _name;

        private void Awake()
        {
            if (instance && !Equals(instance, this))
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        private void Start()
        {
            threeFiguresComplete = -1;
            _center = new Vector3(0, 0, 3);
            _endTarget = new Vector3(15, 0, 3);
            WinBobbles.instance.victory = 1;
            for (var i = 0; i < allAnimals.Count; i++)
            {
                var chance = Random.Range(0, allAnimals.Count - 1);
                (allAnimals[i], allAnimals[chance]) = (allAnimals[chance], allAnimals[i]);
            }

            _level3Spawn = GetComponent<Level3Spawn>();
            StartCoroutine(MoveAnimals());

            hint.Initialization(animalCenter, _level3Spawn.activeItem);
            StartCoroutine(hint.StartHint());
        }

        private void Update()
        {
            if (WinBobbles.instance.victory == 0 && _stop == 0)
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
            foreach (var item in allAnimals)
            {
                hint.waitHint = 1;
                RandomItem();
                while (item.transform.position != _center)
                {
                    item.transform.position = Vector3.MoveTowards(item.transform.position, _center, 0.1f);
                    yield return new WaitForSeconds(0.01f);
                }

                animalCenter = item;
                FigureChange();
                figure.GetComponent<SpriteRenderer>().enabled = true;
                task.GetComponent<SpriteRenderer>().enabled = true;
                while (stageMove != 1)
                {
                    yield return new WaitForSeconds(1);
                }

                figure.GetComponent<SpriteRenderer>().enabled = false;
                task.GetComponent<SpriteRenderer>().enabled = false;
                foreach (var item2 in _level3Spawn.activeItem)
                {
                    item2.SetActive(false);
                }

                hint.waitHint = 1;
                while (item.transform.position != _endTarget)
                {
                    item.transform.position = Vector3.MoveTowards(item.transform.position, _endTarget, 0.1f);
                    yield return new WaitForSeconds(0.01f);
                }

                foreach (var item3 in threeFigures)
                {
                    item3.GetComponent<SpriteRenderer>().enabled = false;
                }

                threeFiguresComplete = -1;
                stageMove = 0;
            }

            WinBobbles.instance.victory = 0;
        }

        private void RandomItem()
        {
            for (var i = 0; i < allItem.Count; i++)
            {
                var chance = Random.Range(0, allItem.Count - 1);
                (allItem[i], allItem[chance]) = (allItem[chance], allItem[i]);
            }

            _level3Spawn.SpawnAnimal();
        }

        private void FigureChange()
        {
            if (threeFiguresComplete >= 0)
            {
                threeFigures[threeFiguresComplete].GetComponent<SpriteRenderer>().enabled = true;
                threeFigures[threeFiguresComplete].GetComponent<SpriteRenderer>().sprite = figure.GetComponent<SpriteRenderer>().sprite;
                _name = figure.GetComponent<SpriteRenderer>().sprite.name;
            }

            if (_level3Spawn.activeItem.Count > 2)
            {
                var newItems = _level3Spawn.activeItem.Where(x => x.activeSelf).ToList();
                var newRandom = Random.Range(0, newItems.Count);
                // if (_level3Spawn.activeItem[newRandom] == null)
                // {
                //     FigureChange();
                // }
                // else
                // {
                figure.GetComponent<SpriteRenderer>().sprite = newItems[newRandom].GetComponent<SpriteRenderer>().sprite;
                animalCenter.name = newItems[newRandom].name;
                newItems.RemoveAt(newRandom);
                // }
            }
            else
            {
                stageMove = 1;
            }
        }
    }
}