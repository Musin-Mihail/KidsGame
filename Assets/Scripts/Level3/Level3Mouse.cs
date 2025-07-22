using UnityEngine;

namespace Level3
{
    public class Level3Mouse : MonoBehaviour
    {
        private Camera _camera;
        private Vector3 _position;
        private GameObject _target;
        private const int LayerMask = 1 << 13;
        private const int LayerMask2 = 1 << 9;
        private float _z;
        private Hint _hint;

        private void Start()
        {
            _camera = Camera.main;
            _hint = GetComponent<Hint>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), _camera.transform.forward, Mathf.Infinity, LayerMask);
                if (hit.collider)
                {
                    _z = hit.collider.transform.position.z;
                    _target = hit.collider.gameObject;
                    _position = _target.GetComponent<MoveItem>().startPosition;
                    _hint.waitHint = 1;
                    _target.GetComponent<MoveItem>().state = 0;
                }
            }

            if (Input.GetMouseButtonUp(0) && _target != null)
            {
                var hitCollider = Physics2D.OverlapCircle(_target.transform.position, 0.1f, LayerMask2);
                if (hitCollider)
                {
                    if (hitCollider.name == _target.name)
                    {
                        hitCollider.GetComponent<SoundClickItem>().Play();
                        _target.SetActive(false);
                        Level3Global.instance.nextFigure = 1;
                        Level3Global.instance.threeFiguresComplete++;
                    }
                    else
                    {
                        _target.transform.position = _position;
                    }
                }
                else
                {
                    _target.transform.position = _position;
                }

                _target = null;
            }

#if UNITY_EDITOR
            if (Input.GetMouseButton(0) && _target)
            {
                var vector = _camera.ScreenToWorldPoint(Input.mousePosition);
                vector.z = _z;
                _target.transform.position = vector;
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