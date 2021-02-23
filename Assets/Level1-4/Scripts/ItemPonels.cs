using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPonels : MonoBehaviour 
{
	public List<Image> ItemsIconsArray = new List<Image>();
	public Sprite baseSprite;

	public int currentIndex;


	// Use this for initialization
	void Awake () 
	{
		ResetImage();
	}

	public void SetImage(Sprite sprite)
    {
		ItemsIconsArray[currentIndex].sprite = sprite;
		currentIndex++;
    }

	public void ResetImage()
    {
        foreach (var item in ItemsIconsArray)
        {
			item.sprite = baseSprite;
        }

		currentIndex = 0;
    }
}
