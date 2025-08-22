using UnityEngine;

namespace Core
{
    /// <summary>
    /// Управляет воспроизведением звука в игре.
    /// Реализован как синглтон, чтобы обеспечить единственный экземпляр.
    /// </summary>
    public class AudioManager : Singleton<AudioManager>
    {
        [Tooltip("Звуковой эффект для клика или успешного перетаскивания предмета.")]
        [SerializeField] private AudioClip clickSound;
        private const float ClickVolume = 0.4f;
        private AudioSource _audioSource;

        protected override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Воспроизводит стандартный звук клика/перетаскивания.
        /// </summary>
        public void PlayClickSound()
        {
            if (clickSound)
            {
                _audioSource.PlayOneShot(clickSound, ClickVolume);
            }
            else
            {
                Debug.LogWarning("Звук клика (clickSound) не назначен в AudioManager.");
            }
        }
    }
}