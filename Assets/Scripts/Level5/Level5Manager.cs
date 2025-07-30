using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Level5
{
    public class Level5Manager : BaseLevelManager<Level5Manager>
    {
        [Header("Настройки уровня 5")]
        [Tooltip("Список всех устриц на сцене, которые будут спаунить фигуры.")]
        public List<Level5SpawnOyster> oysters = new();
        [Tooltip("Список спрайтов для анимации открытия устрицы.")]
        public List<Sprite> oysterStages = new();
        [Tooltip("Префаб сундука, который анимируется в конце.")]
        public GameObject chest;
        [Tooltip("Префаб открытого сундука.")]
        public GameObject openChest;
        private int _itemsToWin;
        private bool _isVictoryTriggered;

        protected override void Start()
        {
            if (WinBobbles.instance)
            {
                _itemsToWin = allItems.Count;
                WinBobbles.instance.victory = _itemsToWin;
            }

            base.Start();
        }

        private void Update()
        {
            if (_isVictoryTriggered || !WinBobbles.instance || WinBobbles.instance.victory != 0) return;
            _isVictoryTriggered = true;
            StartCoroutine(WinAnimation());
        }

        protected override void InitializeSpawner()
        {
            foreach (var oyster in oysters)
            {
                oyster.InitialSpawn();
            }

            Shuffle(allTargets);
            var spawner = GetComponent<Level5Spawner>();
            if (spawner)
            {
                spawner.SpawnTargets(allTargets);
            }
        }

        protected override void InitializeHint()
        {
            if (!hint) return;
            var spawner = GetComponent<Level5Spawner>();
            if (!spawner) return;
            hint.Initialization(spawner.spawnedTargets, spawner.activeItem);
            StartCoroutine(hint.StartHint());
        }

        /// <summary>
        /// Возвращает следующую цветную фигуру для спауна.
        /// </summary>
        public GameObject GetNextFigureToSpawn()
        {
            if (allItems.Count == 0) return null;
            var figure = allItems[0];
            allItems.RemoveAt(0);
            return figure;
        }

        /// <summary>
        /// Победная анимация.
        /// </summary>
        private IEnumerator WinAnimation()
        {
            var spawner = GetComponent<Level5Spawner>();
            if (spawner)
            {
                foreach (var target in spawner.spawnedTargets)
                {
                    if (target) target.gameObject.SetActive(false);
                    yield return new WaitForSeconds(0.05f);
                }
            }

            foreach (var oyster in oysters)
            {
                if (oyster) oyster.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.05f);
            }

            if (chest)
            {
                chest.GetComponent<Animator>().enabled = true;
                yield return new WaitForSeconds(1.5f);
                chest.SetActive(false);
            }

            if (openChest)
            {
                openChest.SetActive(true);
            }
        }
    }
}