using UnityEngine;

namespace Level5
{
    public class Level5Mouse : MonoBehaviour
    {
        private Camera _camera;
        private GameObject _gameObject;
        public Vector3 position;
        private const int LayerMask = 1 << 13;
        private const int LayerMask2 = 1 << 9;
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
                    position = _gameObject.transform.position;
                    Level5Global.WaitHint = 1;
                }
            }

            if (Input.GetMouseButtonUp(0) && _gameObject != null)
            {
                var hitColliders = Physics2D.OverlapCircle(_gameObject.transform.position, 0.1f, LayerMask2);
                if (hitColliders)
                {
                    if (hitColliders.tag == _gameObject.tag)
                    {
                        hitColliders.GetComponent<SoundClickItem>().Play();
                        var newVector3 = hitColliders.transform.position;
                        newVector3.z += 0.5f;
                        Instantiate(Resources.Load<ParticleSystem>("Bubbles"), newVector3, Quaternion.Euler(-90, -40, 0));
                        _gameObject.SetActive(false);
                        var parentOyster = _gameObject.transform.parent.gameObject;
                        StartCoroutine(parentOyster.GetComponent<Level5SpawnOyster>().ChangeStage());
                        hitColliders.GetComponent<SpriteRenderer>().sprite = _gameObject.GetComponent<SpriteRenderer>().sprite;
                        hitColliders.tag = "Untagged";
                        WinBobbles.instance.victory--;
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
    }
}