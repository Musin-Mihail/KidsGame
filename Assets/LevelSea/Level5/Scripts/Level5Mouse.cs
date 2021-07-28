using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5Mouse : MonoBehaviour
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
                Level5Global.WaitHint = 1;
            }
        } 
        if(Input.GetMouseButtonUp(0) && _gameObject != null)
        {
            Collider2D hitColliders = Physics2D.OverlapCircle(_gameObject.transform.position, 0.1f, layerMask2);
            if(hitColliders != null)
            {
                if(hitColliders.tag == _gameObject.tag)
                {
                    hitColliders.GetComponent<SoundClickItem>().Play();
                    var newVector3 = hitColliders.transform.position;
                    newVector3.z += 0.5f;
                    Instantiate(Resources.Load<ParticleSystem>("Bubbles"), newVector3, Quaternion.Euler(-90,-40,0));
                    _gameObject.SetActive(false);
                    var parentOyster = _gameObject.transform.parent.gameObject;
                    StartCoroutine(parentOyster.GetComponent<Level5SpawnOyster>().ChangeStage());
                    hitColliders.GetComponent<SpriteRenderer>().sprite = _gameObject.GetComponent<SpriteRenderer>().sprite;
                    hitColliders.tag = "Untagged";
                    WinBobbles.Victory --;
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