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
        private Vector3 _startPosition;
        private Hint _hint;

        private PlayerControls _playerControls;

        public void Initialization(Hint hint)
        {
            _playerControls = new PlayerControls();
            _playerControls.Gameplay.Enable();

            _playerControls.Gameplay.Click.performed += OnClickPerformed;
            _playerControls.Gameplay.Click.canceled += OnClickCanceled;

            _camera = Camera.main;
            _hint = hint;
        }

        private void OnDestroy()
        {
            _playerControls.Gameplay.Click.performed -= OnClickPerformed;
            _playerControls.Gameplay.Click.canceled -= OnClickCanceled;
            _playerControls.Gameplay.Disable();
        }

        private void OnClickPerformed(InputAction.CallbackContext context)
        {
            var screenPosition = _playerControls.Gameplay.PointerPosition.ReadValue<Vector2>();

            var hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(screenPosition), _camera.transform.forward, Mathf.Infinity, DraggableLayerMask);
            if (!hit.collider) return;
            _gameObject = hit.collider.gameObject;
            _startPosition = _gameObject.GetComponent<MoveItem>().endPosition;

            if (_hint)
            {
                _hint.waitHint = 1;
            }

            _gameObject.GetComponent<MoveItem>().state = 0;
        }

        private void OnClickCanceled(InputAction.CallbackContext context)
        {
            if (!_gameObject) return;

            var hitCollider = Physics2D.OverlapCircle(_gameObject.transform.position, 0.1f, TargetLayerMask);
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
            if (!_gameObject || !_playerControls.Gameplay.Drag.IsPressed()) return;
            var screenPosition = _playerControls.Gameplay.PointerPosition.ReadValue<Vector2>();

            var worldPosition = _camera.ScreenToWorldPoint(screenPosition);
            _gameObject.transform.position = worldPosition;
        }

        private void HandleSuccessfulDrop(Collider2D hitCollider)
        {
            var newVector3 = hitCollider.transform.position;
            newVector3.z -= 0.5f;
            Instantiate(Resources.Load<ParticleSystem>("BubblesLevel1"), newVector3, Quaternion.Euler(-90, -40, 0));

            hitCollider.GetComponent<SpriteRenderer>().sprite = _gameObject.GetComponent<SpriteRenderer>().sprite;
            hitCollider.GetComponent<SoundClickItem>()?.Play();

            var level1Spawn = Level1Global.instance.level1Spawn;
            if (level1Spawn)
            {
                for (var i = 0; i < level1Spawn.activeItem.Count; i++)
                {
                    if (level1Spawn.activeItem[i] == null || level1Spawn.activeItem[i].name != _gameObject.name) continue;
                    level1Spawn.SpawnAnimal(i);
                    break;
                }
            }

            _gameObject.SetActive(false);
            if (WinBobbles.instance)
            {
                WinBobbles.instance.victory--;
            }
        }
    }
}