using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputController
{
    /// <summary>
    /// Определяет метод сравнения перетаскиваемого объекта с целью.
    /// </summary>
    public enum ComparisonType
    {
        CheckByName,
        CheckByTag
    }

    /// <summary>
    /// Универсальный контроллер, отвечающий за механику перетаскивания объектов (Drag and Drop).
    /// Он содержит всю общую логику и не зависит от конкретного уровня.
    /// Версия с поддержкой как мыши, так и сенсорного ввода.
    /// </summary>
    public class DragAndDropController : MonoBehaviour
    {
        public event Action<GameObject, Collider2D> OnSuccessfulDrop;

        [Header("Логика сравнения")]
        [Tooltip("Выберите, как сравнивать перетаскиваемый объект с целью.")]
        [SerializeField] private ComparisonType comparisonMethod = ComparisonType.CheckByName;

        [Header("Настройки слоев")]
        [Tooltip("Слой, на котором находятся объекты, которые можно перетаскивать.")]
        [SerializeField] private LayerMask draggableLayerMask = 1 << 13;
        [Tooltip("Слой, на котором находятся цели для перетаскиваемых объектов.")]
        [SerializeField] private LayerMask targetLayerMask = 1 << 9;

        private Camera _camera;
        private GameObject _draggedObject;
        private Vector3 _startPosition;
        private Hint _hint;

        private PlayerControls _playerControls;

        private void Awake()
        {
            _playerControls = new PlayerControls();
            _camera = Camera.main;
            _hint = GetComponent<Hint>();
        }

        private void OnEnable()
        {
            _playerControls.Gameplay.Enable();
            _playerControls.Gameplay.Click.performed += OnClickPerformed;
            _playerControls.Gameplay.Click.canceled += OnClickCanceled;
        }

        private void OnDisable()
        {
            _playerControls.Gameplay.Click.performed -= OnClickPerformed;
            _playerControls.Gameplay.Click.canceled -= OnClickCanceled;
            _playerControls.Gameplay.Disable();
        }

        private void Update()
        {
            if (!_draggedObject) return;

            var screenPosition = _playerControls.Gameplay.PointerPosition.ReadValue<Vector2>();
            var worldPosition = _camera.ScreenToWorldPoint(screenPosition);
            _draggedObject.transform.position = worldPosition;
        }

        /// <summary>
        /// Вызывается при нажатии кнопки мыши/касании экрана.
        /// </summary>
        private void OnClickPerformed(InputAction.CallbackContext context)
        {
            var screenPosition = _playerControls.Gameplay.PointerPosition.ReadValue<Vector2>();
            var hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(screenPosition), _camera.transform.forward, Mathf.Infinity, draggableLayerMask);
            if (!hit.collider) return;
            _draggedObject = hit.collider.gameObject;
            var moveItem = _draggedObject.GetComponent<MoveItem>();
            if (moveItem)
            {
                _startPosition = moveItem.endPosition;
                moveItem.state = 0;
            }
            else
            {
                _startPosition = _draggedObject.transform.position;
            }

            if (_hint)
            {
                _hint.waitHint = 1;
            }
        }

        /// <summary>
        /// Вызывается при отпускании кнопки мыши/прекращении касания.
        /// </summary>
        private void OnClickCanceled(InputAction.CallbackContext context)
        {
            if (!_draggedObject) return;
            var hitCollider = Physics2D.OverlapCircle(_draggedObject.transform.position, 0.1f, targetLayerMask);
            if (hitCollider)
            {
                var isMatch = comparisonMethod switch
                {
                    ComparisonType.CheckByName => hitCollider.gameObject.name == _draggedObject.name,
                    ComparisonType.CheckByTag => hitCollider.CompareTag(_draggedObject.tag),
                    _ => false
                };
                if (isMatch)
                {
                    OnSuccessfulDrop?.Invoke(_draggedObject, hitCollider);
                }
                else
                {
                    _draggedObject.transform.position = _startPosition;
                }
            }
            else
            {
                _draggedObject.transform.position = _startPosition;
            }

            _draggedObject = null;
        }
    }
}