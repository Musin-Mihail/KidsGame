using UnityEngine;

public class Level2Mouse : MonoBehaviour
{
    private Camera _camera;
    private GameObject _gameObject;
    private const int LayerMask = 1 << 13;
    private const int LayerMask2 = 1 << 9;
    private float _z;
    private Vector3 _position;
    private Vector3 _bigScale;
    public Transform normalScale;
    public Transform normalScaleWindows3;
    public Transform normalScaleWindows1;

    private void Start()
    {
        _camera = Camera.main;
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
                _bigScale = _gameObject.transform.localScale;
                if (_gameObject.name == "Window3")
                {
                    _gameObject.transform.localScale = normalScaleWindows3.lossyScale;
                }
                else if (_gameObject.name == "Window1")
                {
                    _gameObject.transform.localScale = normalScaleWindows1.lossyScale;
                }
                else
                {
                    _gameObject.transform.localScale = normalScale.lossyScale;
                }

                _position = hit.collider.GetComponent<MoveItem>().StartPosition;
                Level2Global.WaitHint = 1;
                hit.collider.GetComponent<MoveItem>().State = 0;
            }
        }

        if (Input.GetMouseButtonUp(0) && _gameObject != null)
        {
            var hitCollider = Physics2D.OverlapCircle(_gameObject.transform.position, 0.1f, LayerMask2);
            if (hitCollider)
            {
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
                    WinBobbles.Victory--;
                }
                else
                {
                    _gameObject.transform.position = _position;
                    _gameObject.transform.localScale = _bigScale;
                }
            }
            else
            {
                _gameObject.transform.position = _position;
                _gameObject.transform.localScale = _bigScale;
            }

            _gameObject = null;
        }

#if UNITY_EDITOR
        if (Input.GetMouseButton(0) && _gameObject != null)
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