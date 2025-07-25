using Core;
using UnityEngine;

namespace Level10
{
    public class Level10Mouse : MonoBehaviour
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
                    Position = _gameObject.transform.position;
                    Level10Global.WaitHint = 1;
                    _gameObject.GetComponent<MoveItem>().state = 0;
                }
            }

            if (Input.GetMouseButtonUp(0) && _gameObject)
            {
                var hitCollider = Physics2D.OverlapCircle(_gameObject.transform.position, 0.1f, LayerMask2);
                if (hitCollider)
                {
                    if (hitCollider.tag == _gameObject.tag)
                    {
                        AudioManager.instance.PlayClickSound();
                        var allChildren = hitCollider.GetComponentsInChildren<Transform>();
                        foreach (var item in allChildren)
                        {
                            if (item.name == _gameObject.name)
                            {
                                item.GetComponent<SpriteRenderer>().enabled = true;
                                Level10Global.AllBusyPlace.Add(item.gameObject);
                                break;
                            }
                        }

                        Level10Global.next--;
                        _gameObject.SetActive(false);
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