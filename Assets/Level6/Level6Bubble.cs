using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level6Bubble : MonoBehaviour
{
    SpriteRenderer _SpriteRenderer;
    AudioSource _AudioSource;

    void OnMouseUp()
    {
        _AudioSource = GetComponent<AudioSource>();
        Destroy(GetComponent<CircleCollider2D>());
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Bursting());
    }
    IEnumerator Bursting() 
    {
        _AudioSource.Play();
        _SpriteRenderer.sprite = Level6Global.SpriteBubbleStatic[0];
        yield return new WaitForSeconds(0.2f);
        _SpriteRenderer.sprite = Level6Global.SpriteBubbleStatic[1];
        yield return new WaitForSeconds(0.2f);
        _SpriteRenderer.sprite = Level6Global.SpriteBubbleStatic[2];
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}