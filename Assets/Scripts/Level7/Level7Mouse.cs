using Core;
using UnityEngine;

namespace Level7
{
    public class Level7Mouse : MonoBehaviour
    {
        public Vector3 Position;
        private Camera _camera;
        private GameObject _gameObject;
        private const int LayerMask = 1 << 13;
        private const int LayerMask2 = 1 << 9;
        private float _z;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), _camera.transform.forward, Mathf.Infinity, LayerMask);
                if (hit.collider)
                {
                    _z = hit.collider.transform.position.z;
                    _gameObject = hit.collider.gameObject;
                    Position = _gameObject.GetComponent<MoveItem>().startPosition;
                    Level7Global.WaitHint = 1;
                    _gameObject.GetComponent<MoveItem>().state = 0;
                }
            }

            if (Input.GetMouseButtonUp(0) && _gameObject != null)
            {
                var hitCollider = Physics2D.OverlapCircle(_gameObject.transform.position, 0.1f, LayerMask2);
                if (hitCollider)
                {
                    if (hitCollider.name == _gameObject.name)
                    {
                        AudioManager.instance.PlayClickSound();
                        hitCollider.GetComponent<SpriteRenderer>().sprite = _gameObject.GetComponent<SpriteRenderer>().sprite;
                        hitCollider.transform.localScale = _gameObject.transform.localScale * 100;
                        _gameObject.SetActive(false);
                        Level7Global.NextFigure = 1;
                    }
                    else
                    {
                        _gameObject.transform.position = Position;
                    }
                }
                else
                {
                    _gameObject.transform.position = Position;
                }

                _gameObject = null;
            }

#if UNITY_EDITOR
            if (Input.GetMouseButton(0) && _gameObject)
            {
                var vector = _camera.ScreenToWorldPoint(Input.mousePosition);
                vector.z = _z;
                _gameObject.transform.position = vector;
            }

#else
            if (Input.touchCount > 0 && _gameObject)
            {
                var vector = _camera.ScreenToWorldPoint(Input.GetTouch(0).position);
                vector.z = _z;
                _gameObject.transform.position = vector;
            }
#endif
        }
    }
}