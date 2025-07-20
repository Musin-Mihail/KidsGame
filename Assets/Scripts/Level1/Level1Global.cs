using System.Collections.Generic;
using UnityEngine;

namespace Level1
{
    public class Level1Global : MonoBehaviour
    {
        public static Level1Global instance { get; private set; }
        public Hint hint;
        public List<GameObject> allAnimals = new();
        public List<GameObject> allEmpty = new();

        [HideInInspector] public Level1Spawn level1Spawn;

        private int _check;

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
            for (var i = 0; i < allAnimals.Count; i++)
            {
                var chance = Random.Range(0, allAnimals.Count - 1);
                (allAnimals[i], allAnimals[chance]) = (allAnimals[chance], allAnimals[i]);
            }

            level1Spawn = gameObject.GetComponent<Level1Spawn>();
            level1Spawn.Initialization();

            hint = gameObject.GetComponent<Hint>();
            hint.Initialization(allEmpty, level1Spawn.activeItem);
            StartCoroutine(hint.StartHint());
        }
    }
}