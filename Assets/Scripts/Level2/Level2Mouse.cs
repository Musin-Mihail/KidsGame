using UnityEngine;

namespace Level2
{
    public class Level2Mouse : MonoBehaviour
    {
        private Camera _camera;
        private GameObject _gameObject;
        private const int LayerMask = 1 << 13;
        private const int LayerMask2 = 1 << 9;
        private float _z;
        private Vector3 _position;
        private Vector3 _bigScale;
        private Hint _hint;

        public void Initialization()
        {
            _camera = Camera.main;
            _hint = gameObject.GetComponent<Hint>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), _camera.transform.forward, Mathf.Infinity, LayerMask);
                if (hit.collider)
                {
                    _z = hit.collider.transform.position.z;
                    _gameObject = hit.collider.gameObject;

                    _position = hit.collider.GetComponent<MoveItem>().endPosition;

                    _hint.waitHint = 1;
                    hit.collider.GetComponent<MoveItem>().state = 0;
                }
            }

            if (Input.GetMouseButtonUp(0) && _gameObject != null)
            {
                var hitCollider = Physics2D.OverlapCircle(_gameObject.transform.position, 0.1f, LayerMask2);
                if (hitCollider)
                {
                    Debug.Log(hitCollider.name);
                    Debug.Log(_gameObject.name);

                    if (hitCollider.name == _gameObject.name)
                    {
                        if (hitCollider.name == "Flag")
                        {
                            hitCollider.GetComponent<Animator>().enabled = true;
                        }
                        else
                        {
                            hitCollider.GetComponent<SpriteRenderer>().sprite = _gameObject.GetComponent<SpriteRenderer>().sprite;
                        }

                        hitCollider.GetComponent<SoundClickItem>().Play();
                        _gameObject.SetActive(false);
                        WinBobbles.instance.victory--;
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
            InputUnity();
#else
            InputMobile();
#endif
        }

        private void InputUnity()
        {
            if (!Input.GetMouseButton(0) || !_gameObject) return;
            var vector = _camera.ScreenToWorldPoint(Input.mousePosition);
            vector.z = _z;
            _gameObject.transform.position = vector;
        }

        private void InputMobile()
        {
            if (Input.touchCount <= 0 || !_gameObject) return;
            var vector = _camera.ScreenToWorldPoint(Input.GetTouch(0).position);
            vector.z = _z;
            _gameObject.transform.position = vector;
        }
    }
}