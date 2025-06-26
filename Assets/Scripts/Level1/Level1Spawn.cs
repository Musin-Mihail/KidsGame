using System.Collections.Generic;
using UnityEngine;

namespace Level1
{
    public class Level1Spawn : MonoBehaviour
    {
        public List<Transform> spawnPositions;
        public List<GameObject> activeItem;
        public GameObject parent;
        private const float EdgeOffset = 1.34f;
        private Camera _mainCamera;
        private readonly List<Vector3> _leftSidePositions = new();
        private readonly List<Vector3> _rightSidePositions = new();

        private void Awake()
        {
            _mainCamera = Camera.main;
            CalculateEdgePositions();
        }

        private void Start()
        {
            for (var i = 0; i < activeItem.Count; i++)
            {
                SpawnAnimal(i);
            }
        }

        private void CalculateEdgePositions()
        {
            if (!_mainCamera)
            {
                Debug.LogError("Основная камера не найдена. Убедитесь, что у вас есть камера с тегом 'MainCamera'.");
                return;
            }

            if (spawnPositions == null || spawnPositions.Count == 0)
            {
                Debug.LogWarning("Список 'spawnPositions' не заполнен. Невозможно вычислить позиции.");
                return;
            }

            var rightEdgeWorldPoint = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
            _leftSidePositions.Clear();
            _rightSidePositions.Clear();
            foreach (var spawnTransform in spawnPositions)
            {
                if (spawnTransform == null)
                {
                    Debug.LogWarning("В списке 'spawnPositions' есть пустой элемент. Он будет пропущен.");
                    continue;
                }

                var yPos = spawnTransform.position.y;
                var zPos = spawnTransform.position.z;
                var leftPoint = new Vector3(rightEdgeWorldPoint.x - EdgeOffset, yPos, zPos);
                _leftSidePositions.Add(leftPoint);
                var rightPoint = new Vector3(rightEdgeWorldPoint.x + EdgeOffset, yPos, zPos);
                _rightSidePositions.Add(rightPoint);
            }
        }

        public void SpawnAnimal(int number)
        {
            if (Level1Global.Instance.allAnimals.Count > 0)
            {
                var animal = Instantiate(Level1Global.Instance.allAnimals[0], parent.transform, false);
                var moveItem = animal.GetComponent<MoveItem>();
                moveItem.Initialization(_rightSidePositions[number], _leftSidePositions[number]);
                animal.name = Level1Global.Instance.allAnimals[0].name;
                activeItem[number] = animal;
                Level1Global.Instance.allAnimals.RemoveAt(0);
                StartCoroutine(moveItem.StartMove());
            }
        }
    }
}