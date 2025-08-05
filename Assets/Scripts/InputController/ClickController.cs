using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputController
{
    /// <summary>
    /// Контроллер, отвечающий за обработку простых кликов по объектам.
    /// Не зависит от конкретного уровня.
    /// </summary>
    public class ClickController : MonoBehaviour
    {
        public event Action<GameObject> OnObjectClicked;
        [Header("Настройки слоев")]
        [Tooltip("Слой, на котором находятся объекты, по которым можно кликать.")]
        [SerializeField] private LayerMask clickableLayerMask = 1 << 13;
        private Camera _camera;
        private PlayerControls _playerControls;
        private Hint _hint;

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
        }

        private void OnDisable()
        {
            _playerControls.Gameplay.Click.performed -= OnClickPerformed;
            _playerControls.Gameplay.Disable();
        }

        /// <summary>
        /// Вызывается при нажатии кнопки мыши/касании экрана.
        /// </summary>
        private void OnClickPerformed(InputAction.CallbackContext context)
        {
            Debug.Log("OnClickPerformed");
            
            if (!_camera) return;
            var screenPosition = _playerControls.Gameplay.PointerPosition.ReadValue<Vector2>();
            var hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(screenPosition), _camera.transform.forward, Mathf.Infinity, clickableLayerMask);
            if (!hit.collider) return;
            OnObjectClicked?.Invoke(hit.collider.gameObject);
            if (_hint)
            {
                _hint.waitHint = 1;
            }
        }
    }
}