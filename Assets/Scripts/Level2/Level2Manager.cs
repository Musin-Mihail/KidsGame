using System.Collections;
using Core;
using UnityEngine;

namespace Level2
{
    public class Level2Manager : BaseLevelManager<Level2Manager>
    {
        [Header("Настройки уровня 2")]
        public GameObject boat;

        private Vector3 _targetBoat;
        private int _win;
        private Level2Spawner _level2Spawn;

        protected override void Awake()
        {
            base.Awake();
            _level2Spawn = GetComponent<Level2Spawner>();
        }

        protected override void Start()
        {
            if (WinBobbles.instance)
            {
                WinBobbles.instance.victory = 8;
            }

            _targetBoat = new Vector3(-15, 1.1f, 2.89f);
            base.Start();
        }

        private void Update()
        {
            if (WinBobbles.instance.victory != 0 || _win != 0) return;
            _win = 1;
            StartCoroutine(Win());
        }

        private IEnumerator Win()
        {
            while (boat.transform.position != _targetBoat)
            {
                boat.transform.position = Vector3.MoveTowards(boat.transform.position, _targetBoat, 0.1f);
                yield return new WaitForSeconds(0.02f);
            }
        }

        protected override void InitializeSpawner()
        {
            if (_level2Spawn)
            {
                _level2Spawn.Initialization();
            }
        }

        protected override void InitializeHint()
        {
            if (!hint || !_level2Spawn) return;
            hint.Initialization(allTargets, _level2Spawn.activeItem);
            StartCoroutine(hint.StartHint());
        }
    }
}