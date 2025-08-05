using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    public Sprite sprite1;
    public Sprite sprite2;
    private AudioSource _musicAudioSource;

    private void Start()
    {
        if (!Music.instance) return;
        _musicAudioSource = Music.instance.GetComponent<AudioSource>();
        UpdateSprite();
    }

    public void OffSound()
    {
        if (!_musicAudioSource) return;
        _musicAudioSource.mute = !_musicAudioSource.mute;
        UpdateSprite();
    }

    /// <summary>
    /// Обновляет иконку звука в зависимости от состояния mute.
    /// </summary>
    private void UpdateSprite()
    {
        if (_musicAudioSource)
        {
            GetComponent<Image>().sprite = _musicAudioSource.mute ? sprite2 : sprite1;
        }
    }
}