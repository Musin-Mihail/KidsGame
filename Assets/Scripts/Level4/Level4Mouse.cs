using System.Collections;
using UnityEngine;

namespace Level4
{
    public class Level4Mouse : MonoBehaviour
    {
        Camera _camera;
        GameObject _gameObject;
        public Vector3 Position;
        int layerMask = 1 << 13;
        int layerMask2 = 1 << 10;
        float _z;

        void Start()
        {
            _camera = Camera.main;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), _camera.transform.forward, Mathf.Infinity, layerMask);
                if (hit.collider != null)
                {
                    _z = hit.collider.transform.position.z;
                    _gameObject = hit.collider.gameObject;
                    Position = _gameObject.GetComponent<MoveItem>().StartPosition;
                    Level4Global.WaitHint = 1;
                    _gameObject.GetComponent<MoveItem>().State = 0;
                }
            }

            if (Input.GetMouseButtonUp(0) && _gameObject != null)
            {
                Collider2D hitColliders = Physics2D.OverlapCircle(_gameObject.transform.position, 0.1f, layerMask2);
                if (hitColliders != null)
                {
                    if (hitColliders.tag == _gameObject.tag)
                    {
                        Transform[] allChildren = hitColliders.GetComponentsInChildren<Transform>();
                        foreach (var item in allChildren)
                        {
                            if (item.name == _gameObject.name)
                            {
                                _gameObject.GetComponent<BoxCollider2D>().enabled = false;
                                StartCoroutine(MoveAnimal(item, _gameObject));
                                Level4Global._level4Spawn.GetComponent<Level4Spawn>().SearchFreeSpace(_gameObject.name);
                                break;
                            }
                        }
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

            // if(Input.GetMouseButton(0) && _gameObject != null)
            // {
            //     var vector = _camera.ScreenToWorldPoint(Input.mousePosition);
            //     vector.z = _z;
            //     _gameObject.transform.position = vector;
            // } 
            if (Input.touchCount > 0 && _gameObject != null)
            {
                var vector = _camera.ScreenToWorldPoint(Input.GetTouch(0).position);
                vector.z = _z;
                _gameObject.transform.position = vector;
            }
        }

        IEnumerator MoveAnimal(Transform item, GameObject animal)
        {
            item.GetComponent<SoundClickItem>().Play();
            Level4Global.AllCollected.Add(item.gameObject);
            while (animal.transform.position != item.transform.position)
            {
                animal.transform.position = Vector3.MoveTowards(animal.transform.position, item.transform.position, 0.1f);
                yield return new WaitForSeconds(0.01f);
            }

            if (item.tag == "Water")
            {
                var newVector3 = item.transform.position;
                newVector3.z += 0.5f;
                Instantiate(Resources.Load<ParticleSystem>("Bubbles"), newVector3, Quaternion.Euler(-90, -40, 0));
            }

            if (item.name == "Dog")
            {
                Transform[] allChildren = item.GetComponentsInChildren<Transform>();
                foreach (var item2 in allChildren)
                {
                    item2.GetComponent<SpriteRenderer>().enabled = true;
                }
            }

            item.GetComponent<SpriteRenderer>().enabled = true;
            animal.SetActive(false);
            WinBobbles.Victory--;
        }
    }
}