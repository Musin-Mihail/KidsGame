using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundClickItem : MonoBehaviour
{
    AudioSource _AudioSource;
    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        _AudioSource = GetComponent<AudioSource>();
    }
    public void Play()
    {
        _AudioSource.PlayOneShot(Direction.SoundStatic, 0.3F);
    }
}