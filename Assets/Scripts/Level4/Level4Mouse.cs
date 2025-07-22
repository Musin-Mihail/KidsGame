using System.Collections;
using UnityEngine;

namespace Level4
{
    public class Level4Mouse : MonoBehaviour
    {
        private Camera _camera;
        private GameObject _gameObject;
        public Vector3 position;
        private const int LayerMask = 1 << 13;
        private const int LayerMask2 = 1 << 10;
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
                    position = _gameObject.GetComponent<MoveItem>().startPosition;
                    Level4Global.WaitHint = 1;
                    _gameObject.GetComponent<MoveItem>().state = 0;
                }
            }

            if (Input.GetMouseButtonUp(0) && _gameObject)
            {
                var hitColliders = Physics2D.OverlapCircle(_gameObject.transform.position, 0.1f, LayerMask2);
                if (hitColliders)
                {
                    if (hitColliders.tag == _gameObject.tag)
                    {
                        var allChildren = hitColliders.GetComponentsInChildren<Transform>();
                        foreach (var item in allChildren)
                        {
                            if (item.name != _gameObject.name) continue;
                            _gameObject.GetComponent<BoxCollider2D>().enabled = false;
                            StartCoroutine(MoveAnimal(item, _gameObject));
                            Level4Global._level4Spawn.GetComponent<Level4Spawn>().SearchFreeSpace(_gameObject.name);
                            break;
                        }
                    }
                    else
                    {
                        _gameObject.transform.position = position;
                    }
                }
                else
                {
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
            if (Input.touchCount > 0 && _gameObject)
            {
                var vector = _camera.ScreenToWorldPoint(Input.GetTouch(0).position);
                vector.z = _z;
                _gameObject.transform.position = vector;
            }
#endif
        }

        private IEnumerator MoveAnimal(Transform item, GameObject animal)
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
                var allChildren = item.GetComponentsInChildren<Transform>();
                foreach (var item2 in allChildren)
                {
                    item2.GetComponent<SpriteRenderer>().enabled = true;
                }
            }

            item.GetComponent<SpriteRenderer>().enabled = true;
            animal.SetActive(false);
            WinBobbles.instance.victory--;
        }
    }
}