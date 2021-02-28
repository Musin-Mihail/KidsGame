using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstingBubble : MonoBehaviour
{
    public List<Sprite> SpriteBubble = new List<Sprite>();
    SpriteRenderer _SpriteRenderer;
    AudioSource _AudioSource;
    void Start()
    {
        StartCoroutine(DestroyBubble());
    }

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
        _SpriteRenderer.sprite = SpriteBubble[0];
        yield return new WaitForSeconds(0.1f);
        _SpriteRenderer.sprite = SpriteBubble[1];
        yield return new WaitForSeconds(0.1f);
        _SpriteRenderer.sprite = SpriteBubble[2];
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
    IEnumerator DestroyBubble()
    {
        yield return new WaitForSeconds(20);
        Destroy(gameObject);
    }
}