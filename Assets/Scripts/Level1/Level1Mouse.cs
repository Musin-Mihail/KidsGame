using UnityEngine;
using UnityEngine.InputSystem;

namespace Level1
{
    public class Level1Mouse : MonoBehaviour
    {
        private Camera _camera;
        private GameObject _gameObject;
        private const int DraggableLayerMask = 1 << 13;
        private const int TargetLayerMask = 1 << 9;
        private float _z;
        private Vector3 _startPosition;
        private Hint _hint;

        private PlayerControls _playerControls;

        private void Awake()
        {
            _playerControls = new PlayerControls();
            _playerControls.Gameplay.Enable();

            _playerControls.Gameplay.Click.performed += OnClickPerformed;
            _playerControls.Gameplay.Click.canceled += OnClickCanceled;
        }

        private void OnDestroy()
        {
            _playerControls.Gameplay.Click.performed -= OnClickPerformed;
            _playerControls.Gameplay.Click.canceled -= OnClickCanceled;
            _playerControls.Gameplay.Disable();
        }

        private void Start()
        {
            _camera = Camera.main;
            _hint = gameObject.GetComponent<Hint>();
        }

        private void OnClickPerformed(InputAction.CallbackContext context)
        {
            Vector2 screenPosition = _playerControls.Gameplay.PointerPosition.ReadValue<Vector2>();

            var hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(screenPosition), _camera.transform.forward, Mathf.Infinity, DraggableLayerMask);
            if (hit.collider)
            {
                _gameObject = hit.collider.gameObject;
                _z = _gameObject.transform.position.z;
                _startPosition = _gameObject.GetComponent<MoveItem>().endPosition;

                if (_hint != null)
                {
                    _hint.waitHint = 1;
                }

                _gameObject.GetComponent<MoveItem>().state = 0;
            }
        }

        private void OnClickCanceled(InputAction.CallbackContext context)
        {
            if (_gameObject == null) return;

            Collider2D hitCollider = Physics2D.OverlapCircle(_gameObject.transform.position, 0.1f, TargetLayerMask);
            if (hitCollider && hitCollider.name == _gameObject.name)
            {
                HandleSuccessfulDrop(hitCollider);
            }
            else
            {
                _gameObject.transform.position = _startPosition;
            }

            _gameObject = null;
        }

        private void Update()
        {
            if (_gameObject != null && _playerControls.Gameplay.Drag.IsPressed())
            {
                Vector2 screenPosition = _playerControls.Gameplay.PointerPosition.ReadValue<Vector2>();

                var worldPosition = _camera.ScreenToWorldPoint(screenPosition);
                worldPosition.z = _z;
                _gameObject.transform.position = worldPosition;
            }
        }

        private void HandleSuccessfulDrop(Collider2D hitCollider)
        {
            var newVector3 = hitCollider.transform.position;
            newVector3.z -= 0.5f;
            Instantiate(Resources.Load<ParticleSystem>("BubblesLevel1"), newVector3, Quaternion.Euler(-90, -40, 0));

            hitCollider.GetComponent<SpriteRenderer>().sprite = _gameObject.GetComponent<SpriteRenderer>().sprite;
            hitCollider.GetComponent<SoundClickItem>()?.Play();

            var level1Spawn = Level1Global.instance.level1Spawn;
            if (level1Spawn != null)
            {
                for (var i = 0; i < level1Spawn.activeItem.Count; i++)
                {
                    if (level1Spawn.activeItem[i] != null && level1Spawn.activeItem[i].name == _gameObject.name)
                    {
                        level1Spawn.SpawnAnimal(i);
                        break;
                    }
                }
            }

            _gameObject.SetActive(false);
            if (WinBobbles.instance != null)
            {
                WinBobbles.instance.victory--;
            }
        }
    }
}