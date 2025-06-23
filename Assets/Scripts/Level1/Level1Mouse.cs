using UnityEngine;

namespace Level1
{
    public class Level1Mouse : MonoBehaviour
    {
        private Camera _camera;
        private GameObject _gameObject;
        private const int LayerMask = 1 << 13;
        private const int LayerMask2 = 1 << 9;
        private float _z;
        private Vector3 _position;

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
                    _position = hit.collider.GetComponent<MoveItem>().StartPosition;
                    Level1Global.WaitHint = 1;
                    hit.collider.GetComponent<MoveItem>().State = 0;
                }
            }

            if (Input.GetMouseButtonUp(0) && _gameObject != null)
            {
                Collider2D hitCollider = Physics2D.OverlapCircle(_gameObject.transform.position, 0.1f, LayerMask2);
                if (hitCollider)
                {
                    if (hitCollider.name == _gameObject.name)
                    {
                        var newVector3 = hitCollider.transform.position;
                        newVector3.z += 0.5f;
                        Instantiate(Resources.Load<ParticleSystem>("BubblesLevel1"), newVector3, Quaternion.Euler(-90, -40, 0));
                        hitCollider.GetComponent<SpriteRenderer>().sprite = _gameObject.GetComponent<SpriteRenderer>().sprite;
                        hitCollider.GetComponent<SoundClickItem>().Play();
                        var level1Spawn = Level1Global.Level1Spawn.GetComponent<Level1Spawn>();
                        for (var i = 0; i < level1Spawn.SpawnPosition.Count - 1; i++)
                        {
                            if (level1Spawn.SpawnPosition[i] != null)
                            {
                                if (level1Spawn.SpawnPosition[i].name == _gameObject.name)
                                {
                                    level1Spawn.SpawnAnimal(i);
                                }
                            }
                        }

                        _gameObject.SetActive(false);
                        WinBobbles.Victory--;
                    }
                    else
                    {
                        _gameObject.transform.position = _position;
                    }
                }
                else
                {
                    _gameObject.transform.position = _position;
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