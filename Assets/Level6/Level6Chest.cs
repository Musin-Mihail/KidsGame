using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level6Chest : MonoBehaviour
{
    public List<Vector3> CollectedThings = new List<Vector3>();
    public AudioClip Bell;
    public int BusyPlaces = 0;
    AudioSource _AudioSource;
    void Start()
    {
        _AudioSource = GetComponent<AudioSource>();
        CollectedThings.Add(new Vector3(-1.5f,2,1.2f));
        CollectedThings.Add(new Vector3(1.5f,2,1.1f));
        CollectedThings.Add(new Vector3(0,2.5f,1));
    }
    public void PlayBell()
    {
        _AudioSource.PlayOneShot(Bell, 0.7F);
    }
}
