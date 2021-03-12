using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    public Sprite _sprite1;
    public Sprite _sprite2;
    public void OffSound()
    {
        if(AudioListener.volume == 1)
        {
            AudioListener.volume = 0;
            GetComponent<Image>().sprite = _sprite2;
        }
        else
        {
            AudioListener.volume = 1;
            GetComponent<Image>().sprite = _sprite1;
        }
    }
}