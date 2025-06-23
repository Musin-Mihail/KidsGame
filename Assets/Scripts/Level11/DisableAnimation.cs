using UnityEngine;

namespace Level11
{
    public class DisableAnimation : MonoBehaviour
    {
        private void OnEnable()
        {
            GetComponent<Animator>().enabled = false;
            transform.localScale = Level11._scaleStatic.lossyScale;
        }
    }
}