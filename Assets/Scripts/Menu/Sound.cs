using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    public Sprite _sprite1;
    public Sprite _sprite2;
    private GameObject _music;

    private void Start()
    {
        _music = GameObject.Find("Music");
        if (_music.GetComponent<AudioSource>().mute == false)
        {
            GetComponent<Image>().sprite = _sprite1;
        }
        else
        {
            GetComponent<Image>().sprite = _sprite2;
        }
    }

    public void OffSound()
    {
        if (_music.GetComponent<AudioSource>().mute == false)
        {
            _music.GetComponent<AudioSource>().mute = true;
            GetComponent<Image>().sprite = _sprite2;
        }
        else
        {
            _music.GetComponent<AudioSource>().mute = false;
            GetComponent<Image>().sprite = _sprite1;
        }
    }
}