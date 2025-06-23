using UnityEngine;

namespace Level12
{
    public class Level12Item : MonoBehaviour
    {
        public Vector2 _scale;

        private void Start()
        {
            _scale = transform.localScale;
        }
    }
}