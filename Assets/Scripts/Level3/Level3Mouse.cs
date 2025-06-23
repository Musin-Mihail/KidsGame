using UnityEngine;

namespace Level3
{
    public class Level3Mouse : MonoBehaviour
    {
        private Camera _camera;
        private Vector3 _position;
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
                    _position = _gameObject.GetComponent<MoveItem>().StartPosition;
                    Level3Global.WaitHint = 1;
                    _gameObject.GetComponent<MoveItem>().State = 0;
                }
            }

            if (Input.GetMouseButtonUp(0) && _gameObject != null)
            {
                Collider2D hitCollider = Physics2D.OverlapCircle(_gameObject.transform.position, 0.1f, LayerMask2);
                if (hitCollider)
                {
                    if (hitCollider.name == _gameObject.name)
                    {
                        hitCollider.GetComponent<SoundClickItem>().Play();
                        _gameObject.SetActive(false);
                        Level3Global.NextFigure = 1;
                        Level3Global.ThreeFiguresComplete++;
                    }
                    else
                    {
                        _gameObject.transform.position = _position;
                    }
                }
                else
                {
                    _gameObject.transform.position = _position;
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