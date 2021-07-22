using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Mouse : MonoBehaviour
{
    Camera _camera;
    GameObject _gameObject;
    int layerMask = 1 << 13;
    int layerMask2 = 1 << 9;
    float _z;
    Vector3 Position;
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
                Position = hit.collider.GetComponent<MoveItem>().StartPosition;
                Level1Global.WaitHint = 1;
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
                    var newVector3 = hitCollider.transform.position;
                    newVector3.z += 0.5f;
                    Instantiate(Resources.Load<ParticleSystem>("BubblesLevel1"), newVector3, Quaternion.Euler(-90,-40,0));
                    hitCollider.GetComponent<SpriteRenderer>().sprite = _gameObject.GetComponent<SpriteRenderer>().sprite;
                    hitCollider.GetComponent<SoundClickItem>().Play();
                    var _level1Spawn = Level1Global.Level1Spawn.GetComponent<Level1Spawn>();
                    for (int i = 0; i < _level1Spawn.SpawnPosition.Count-1; i++)
                    {
                        if(_level1Spawn.SpawnPosition[i] != null)
                        {
                            if(_level1Spawn.SpawnPosition[i].name == _gameObject.name)
                            {
                                _level1Spawn.SpawnPosition[i] = null;
                            }
                        }
                    }
                    Destroy(_gameObject);
                    _level1Spawn.SpawnItem();
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
        if(Input.GetMouseButton(0) && _gameObject != null)
        {
            var vector = _camera.ScreenToWorldPoint(Input.mousePosition);
            vector.z = _z;
            _gameObject.transform.position = vector;
        }
    }
}