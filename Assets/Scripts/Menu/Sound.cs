using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    public Sprite sprite1;
    public Sprite sprite2;
    private GameObject _music;

    private void Start()
    {
        _music = GameObject.Find("Music");
        GetComponent<Image>().sprite = _music.GetComponent<AudioSource>().mute == false ? sprite1 : sprite2;
    }

    public void OffSound()
    {
        if (_music.GetComponent<AudioSource>().mute == false)
        {
            _music.GetComponent<AudioSource>().mute = true;
            GetComponent<Image>().sprite = sprite2;
        }
        else
        {
            _music.GetComponent<AudioSource>().mute = false;
            GetComponent<Image>().sprite = sprite1;
        }
    }
}