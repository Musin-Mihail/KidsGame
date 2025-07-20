using System.Collections.Generic;
using UnityEngine;

namespace Level1
{
    public class Level1Spawn : MonoBehaviour
    {
        public List<Transform> spawnPositions;
        public List<GameObject> activeItem;
        public GameObject parent;
        private Camera _mainCamera;
        private List<Vector3> _leftSidePositions = new();
        private List<Vector3> _rightSidePositions = new();

        private CalculateEdgePositions _calculateEdgePositions;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _calculateEdgePositions = new CalculateEdgePositions();
            var result = _calculateEdgePositions.Calculate(_mainCamera, spawnPositions);
            if (result == null) return;
            _leftSidePositions = result.Value.side1;
            _rightSidePositions = result.Value.side2;
        }

        private void Start()
        {
            for (var i = 0; i < activeItem.Count; i++)
            {
                SpawnAnimal(i);
            }
        }

        public void SpawnAnimal(int number)
        {
            if (Level1Global.instance.allAnimals.Count <= 0) return;
            var animal = Instantiate(Level1Global.instance.allAnimals[0], parent.transform, false);
            var moveItem = animal.GetComponent<MoveItem>();
            moveItem.Initialization(_rightSidePositions[number], _leftSidePositions[number]);
            animal.name = Level1Global.instance.allAnimals[0].name;
            activeItem[number] = animal;
            Level1Global.instance.allAnimals.RemoveAt(0);
            StartCoroutine(moveItem.StartMove());
        }
    }
}