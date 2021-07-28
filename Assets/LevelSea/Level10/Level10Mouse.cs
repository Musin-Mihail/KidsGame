using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level10Mouse : MonoBehaviour
{
    Camera _camera;
    GameObject _gameObject;
    public Vector3 Position;
    int layerMask = 1 << 13;
    int layerMask2 = 1 << 9;
    float _z;
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
                Position = _gameObject.transform.position;
                Level10Global.WaitHint = 1;
                _gameObject.GetComponent<MoveItem>().State = 0;
            }
        }

        if(Input.GetMouseButtonUp(0) && _gameObject != null)
        {
            Collider2D hitCollider = Physics2D.OverlapCircle(_gameObject.transform.position, 0.1f, layerMask2);
            if(hitCollider != null)
            {
                if(hitCollider.tag == _gameObject.tag)
                {
                    hitCollider.GetComponent<SoundClickItem>().Play();
                    Transform[] allChildren = hitCollider.GetComponentsInChildren<Transform>();
                    foreach (var item in allChildren)
                    {
                        if(item.name == _gameObject.name)
                        {
                            item.GetComponent<SpriteRenderer>().enabled = true;
                            Level10Global.AllBusyPlace.Add(item.gameObject);
                            break;
                        }
                    }
                    Level10Global.next --;
                    _gameObject.SetActive(false);
                }
                else
                {
                    _gameObject.transform.position = Position;
                    _gameObject = null;
                }
            }
            else
            {
                _gameObject.transform.position = Position;
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
