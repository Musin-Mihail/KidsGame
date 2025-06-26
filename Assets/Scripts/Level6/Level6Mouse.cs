using UnityEngine;

namespace Level6
{
    public class Level6Mouse : MonoBehaviour
    {
        public Vector3 Position;

        private Camera _camera;
        private GameObject _gameObject;
        private const int LayerMask = 1 << 7;
        private const int LayerMask2 = 1 << 6;
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
                    Position = _gameObject.GetComponent<MoveItem>().startPosition;
                    Level6Global.WaitHint = 1;
                    _gameObject.GetComponent<MoveItem>().state = 0;
                }
            }

            if (Input.GetMouseButtonUp(0) && _gameObject)
            {
                var hitCollider = Physics2D.OverlapCircle(_gameObject.transform.position, 0.1f, LayerMask2);
                if (hitCollider)
                {
                    if (hitCollider.tag == _gameObject.tag)
                    {
                        hitCollider.GetComponent<SoundClickItem>().Play();
                        var place = hitCollider.GetComponent<Level6Chest>().BusyPlaces;
                        var go = new GameObject();
                        go.transform.parent = hitCollider.transform;
                        go.transform.localPosition = hitCollider.GetComponent<Level6Chest>().CollectedThings[place];
                        go.transform.localScale = new Vector3(0.75f, 0.75f, 1);
                        go.AddComponent<SpriteRenderer>();
                        go.GetComponent<SpriteRenderer>().sprite = _gameObject.GetComponent<SpriteRenderer>().sprite;
                        go.AddComponent<WinUp>();
                        Level6Global.AllCollectedStars.Add(go);
                        var newVector3 = go.transform.position;
                        newVector3.z = 2.5f;
                        Instantiate(Resources.Load<ParticleSystem>("Bubbles"), newVector3, Quaternion.Euler(-90, -40, 0));
                        hitCollider.GetComponent<Level6Chest>().BusyPlaces++;
                        _gameObject.SetActive(false);
                        Level6Global._level6Spawn.GetComponent<Level6Spawn>().SearchFreeSpace();
                        WinBobbles.Instance.Victory--;
                    }
                    else
                    {
                        _gameObject.GetComponent<MoveItem>().state = 1;
                        _gameObject.transform.position = Position;
                    }
                }
                else
                {
                    _gameObject.GetComponent<MoveItem>().state = 1;
                    _gameObject.transform.position = Position;
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
    }
}