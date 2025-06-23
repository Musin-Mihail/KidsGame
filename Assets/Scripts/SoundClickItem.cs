using UnityEngine;

public class SoundClickItem : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        gameObject.AddComponent<AudioSource>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        _audioSource.PlayOneShot(Direction.SoundStatic, 0.3F);
    }
}