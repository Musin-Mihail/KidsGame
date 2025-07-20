using System.Collections.Generic;
using UnityEngine;

namespace Level1
{
    public class Level1Global : MonoBehaviour
    {
        public static Level1Global instance { get; private set; }
        public List<GameObject> allAnimals = new();
        public List<GameObject> allEmpty = new();

        [HideInInspector] public Level1Spawn level1Spawn;

        private int _check;
        private Hint _hint;

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
            level1Spawn = gameObject.GetComponent<Level1Spawn>();
            WinBobbles.instance.victory = allEmpty.Count;
            for (var i = 0; i < allAnimals.Count; i++)
            {
                var chance = Random.Range(0, allAnimals.Count - 1);
                (allAnimals[i], allAnimals[chance]) = (allAnimals[chance], allAnimals[i]);
            }

            _hint = gameObject.GetComponent<Hint>();
            _hint.Initialization(allEmpty, level1Spawn.activeItem);
            StartCoroutine(_hint.StartHint());
        }
    }
}