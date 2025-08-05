using UnityEngine;

namespace Level12
{
    public class Level12Item : MonoBehaviour
    {
        public Vector2 scale;

        private void Start()
        {
            scale = transform.localScale;
        }
    }
}