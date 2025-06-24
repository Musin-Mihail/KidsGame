using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstingBubble : MonoBehaviour
{
    public List<Sprite> spriteBubble = new();
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;

    private void Start()
    {
        StartCoroutine(DestroyBubble());
    }

    private void OnMouseUp()
    {
        _audioSource = GetComponent<AudioSource>();
        Destroy(GetComponent<CircleCollider2D>());
        _spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Bursting());
    }

    private IEnumerator Bursting()
    {
        _audioSource.Play();
        _spriteRenderer.sprite = spriteBubble[0];
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.sprite = spriteBubble[1];
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.sprite = spriteBubble[2];
        yield return new WaitForSeconds(0.1f);
        WinBobbles.Instance.Count--;
        Destroy(gameObject);
    }

    private IEnumerator DestroyBubble()
    {
        yield return new WaitForSeconds(10);
        WinBobbles.Instance.Count--;
        Destroy(gameObject);
    }
}