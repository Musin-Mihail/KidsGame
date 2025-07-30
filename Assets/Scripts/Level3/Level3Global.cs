using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Level3
{
    public class Level3Global : BaseLevelManager<Level3Global>
    {
        [Header("Настройки уровня 3")]
        public List<GameObject> threeFigures = new();
        public List<GameObject> allAnimals = new();
        public GameObject task;
        public GameObject figure;

        [HideInInspector] public GameObject animalCenter;
        [HideInInspector] public int stageMove;
        [HideInInspector] public int threeFiguresComplete;

        private Vector3 _center;
        private Vector3 _endTarget;
        private int _stop;
        private Level3Spawn _level3Spawn;

        protected override void Awake()
        {
            base.Awake();
            _level3Spawn = GetComponent<Level3Spawn>();
        }

        protected override void Start()
        {
            threeFiguresComplete = 0;
            _center = new Vector3(0, 0, 3);
            _endTarget = new Vector3(15, 0, 3);
            WinBobbles.instance.victory = 18;
            Shuffle(allAnimals);
            StartCoroutine(MoveAnimals());
            StartCoroutine(hint.StartHint());
        }

        private void Update()
        {
            if (WinBobbles.instance.victory == 0 && _stop == 0)
            {
                _stop = 1;
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
                ChangeAnimal();
                figure.gameObject.SetActive(true);
                task.gameObject.SetActive(true);
                while (stageMove != 1)
                {
                    yield return new WaitForSeconds(1);
                }

                figure.gameObject.SetActive(false);
                task.gameObject.SetActive(false);
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
                    item3.gameObject.SetActive(false);
                }

                threeFiguresComplete = 0;
                stageMove = 0;
            }

            WinBobbles.instance.victory = 0;
        }

        private void RandomItem()
        {
            Shuffle(allItems);
            _level3Spawn.SpawnAnimal();
        }

        private void ChangeAnimal()
        {
            var activeItems = _level3Spawn.activeItem.Where(item => item.activeSelf).ToList();
            if (activeItems.Count == 0) return;
            var randomIndex = Random.Range(0, activeItems.Count);
            var randomAnimal = activeItems[randomIndex];
            figure.GetComponent<SpriteRenderer>().sprite = randomAnimal.GetComponent<SpriteRenderer>().sprite;
            animalCenter.name = randomAnimal.name;
            hint.Initialization(animalCenter, _level3Spawn.activeItem);
        }

        public void ChangeFigure()
        {
            threeFigures[threeFiguresComplete].gameObject.SetActive(true);
            threeFigures[threeFiguresComplete].GetComponent<SpriteRenderer>().sprite = figure.GetComponent<SpriteRenderer>().sprite;
            threeFiguresComplete++;
            if (threeFiguresComplete == 3)
            {
                stageMove = 1;
            }
            else
            {
                ChangeAnimal();
            }
        }

        protected override void InitializeSpawner()
        {
            /* Управляется вручную */
        }

        protected override void InitializeHint()
        {
            /* Управляется вручную */
        }
    }
}