using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level2
{
    public class Level2Global : MonoBehaviour
    {
        public static Level2Global instance { get; private set; }

        public GameObject boat;
        public Hint hint;
        public List<GameObject> allItem = new();
        public List<GameObject> allEmpty = new();

        private Vector3 _targetBoat;
        private int _hintTime;
        private int _win;
        private Level2Spawn _level2Spawn;

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
            WinBobbles.instance.victory = allEmpty.Count;
            for (var i = 0; i < allItem.Count; i++)
            {
                var chance = Random.Range(0, allItem.Count - 1);
                (allItem[i], allItem[chance]) = (allItem[chance], allItem[i]);
            }

            _targetBoat = new Vector3(-15, 1.1f, 2.89f);

            _level2Spawn = gameObject.GetComponent<Level2Spawn>();
            _level2Spawn.Initialization();

            hint.Initialization(allEmpty, _level2Spawn.activeItem);
            StartCoroutine(hint.StartHint());
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
    }
}