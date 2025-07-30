using UnityEngine;

namespace Core
{
    /// <summary>
    /// Управляет воспроизведением звука в игре.
    /// Реализован как синглтон, чтобы обеспечить единственный экземпляр.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance { get; private set; }

        [Tooltip("Звуковой эффект для клика или успешного перетаскивания предмета.")]
        [SerializeField] private AudioClip clickSound;

        [Tooltip("Громкость звука клика.")]
        [SerializeField] [Range(0f, 1f)] private float clickVolume = 0.3f;

        private AudioSource _audioSource;

        private void Awake()
        {
            if (instance && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            _audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Воспроизводит стандартный звук клика/перетаскивания.
        /// </summary>
        public void PlayClickSound()
        {
            if (clickSound)
            {
                _audioSource.PlayOneShot(clickSound, clickVolume);
            }
            else
            {
                Debug.LogWarning("Звук клика (clickSound) не назначен в AudioManager.");
            }
        }
    }
}