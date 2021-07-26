using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level6Mouse : MonoBehaviour
{
    Camera _camera;
    GameObject _gameObject;
    public Vector3 Position;
    int layerMask = 1 << 7;
    int layerMask2 = 1 << 6;
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
                Position = _gameObject.GetComponent<MoveItem>().StartPosition;
                Level6Global.WaitHint = 1;
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
                    var place = hitCollider.GetComponent<Level6Chest>().BusyPlaces;
                    var GO = new GameObject();
                    GO.transform.parent = hitCollider.transform;
                    GO.transform.localPosition = hitCollider.GetComponent<Level6Chest>().CollectedThings[place];
                    GO.transform.localScale = new Vector3(0.75f, 0.75f, 1);
                    GO.AddComponent<SpriteRenderer>();
                    GO.GetComponent<SpriteRenderer>().sprite = _gameObject.GetComponent<SpriteRenderer>().sprite;
                    GO.AddComponent<WinUp>();
                    Level6Global.AllCollectedStars.Add(GO);
                    var newVector3 = GO.transform.position;
                    newVector3.z = 2.5f;
                    Instantiate(Resources.Load<ParticleSystem>("Bubbles"), newVector3, Quaternion.Euler(-90,-40,0));
                    hitCollider.GetComponent<Level6Chest>().BusyPlaces++;
                    _gameObject.SetActive(false);
                    Level6Global._level6Spawn.GetComponent<Level6Spawn>().SearchFreeSpace();
                    WinBobbles.Victory --;
                }
                else
                {
                    _gameObject.GetComponent<MoveItem>().State = 1;
                    _gameObject.transform.position = Position;
                    _gameObject = null;
                }
            }
            else
            {
                _gameObject.GetComponent<MoveItem>().State = 1;
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