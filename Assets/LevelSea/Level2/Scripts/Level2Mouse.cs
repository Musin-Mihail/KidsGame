using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Mouse : MonoBehaviour
{
    Camera _camera;
    GameObject _gameObject;
    int layerMask = 1 << 13;
    int layerMask2 = 1 << 9;
    float _z;
    Vector3 Position;
    Vector3 _bigScale;
    public Transform _normalScale;
    public Transform _normalScaleWindows3;
    public Transform _normalScaleWindows1;
    void Start()
    {
        _camera = Camera.main;
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), _camera.transform.forward, Mathf.Infinity, layerMask);
            if (hit.collider != null)
            {
                _z = hit.collider.transform.position.z;
                _gameObject = hit.collider.gameObject;
                _bigScale = _gameObject.transform.localScale;
                if(_gameObject.name == "Window3")
                {
                    _gameObject.transform.localScale = _normalScaleWindows3.lossyScale;
                }
                else if (_gameObject.name == "Window1")
                {
                    _gameObject.transform.localScale = _normalScaleWindows1.lossyScale;
                }
                else
                {
                    _gameObject.transform.localScale = _normalScale.lossyScale;
                }
                Position = hit.collider.GetComponent<MoveItem>().StartPosition;
                Level2Global.WaitHint = 1;
                hit.collider.GetComponent<MoveItem>().State = 0;
            }
        }
        if(Input.GetMouseButtonUp(0) && _gameObject != null)
        {
            Collider2D hitCollider = Physics2D.OverlapCircle(_gameObject.transform.position, 0.1f, layerMask2);
            if(hitCollider != null)
            {
                if(hitCollider.name == _gameObject.name)
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
                    WinBobbles.Victory --;
                }
                else
                {
                    _gameObject.transform.position = Position;
                    _gameObject.transform.localScale = _bigScale;
                    _gameObject = null;
                }
            }
            else
            {
                _gameObject.transform.position = Position;
                _gameObject.transform.localScale = _bigScale;
                _gameObject = null;
            }
        }
        // if(Input.GetMouseButton(0) && _gameObject != null)
        // {
        //     var vector = _camera.ScreenToWorldPoint(Input.mousePosition);
        //     vector.z = _z;
        //     _gameObject.transform.position = vector;
        // }
        if(Input.touchCount > 0)
        {
            var vector = _camera.ScreenToWorldPoint(Input.GetTouch(0).position);
            vector.z = _z;
            _gameObject.transform.position = vector;
        }
    }
}