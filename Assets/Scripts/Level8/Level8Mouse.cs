using UnityEngine;

public class Level8Mouse : MonoBehaviour
{
    private Camera _camera;
    private GameObject _gameObject;
    public Vector3 position;
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
                position = _gameObject.transform.position;
                _gameObject.transform.parent.gameObject.GetComponent<Level8>().waitHint = 1;
                _gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
            }
        }

        if (Input.GetMouseButtonUp(0) && _gameObject)
        {
            var hitCollider = Physics2D.OverlapCircle(_gameObject.transform.position, 0.1f, LayerMask2);
            if (hitCollider)
            {
                if (hitCollider.name == _gameObject.name)
                {
                    hitCollider.GetComponent<SoundClickItem>().Play();
                    _gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    _gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    _gameObject.transform.position = hitCollider.transform.position;
                    _gameObject.transform.parent.gameObject.GetComponent<Level8>().countItem--;
                    if (_gameObject.transform.parent.gameObject.GetComponent<Level8>().countItem == 0)
                    {
                        _gameObject.transform.parent.gameObject.GetComponent<Level8>().end = 1;
                    }
                }
                else
                {
                    _gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    _gameObject.transform.position = position;
                }
            }
            else
            {
                _gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                _gameObject.transform.position = position;
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
        if (Input.touchCount > 0 && _gameObject != null)
        {
            var vector = _camera.ScreenToWorldPoint(Input.GetTouch(0).position);
            vector.z = _z;
            _gameObject.transform.position = vector;
        }
#endif
    }
}