using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstingBubble : MonoBehaviour
{
    public List<Sprite> spriteBubble = new();
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;
    private bool _isBursting;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(DestroyBubble());
    }

    private void OnMouseUp()
    {
        if (_isBursting) return;
        _isBursting = true;
        Destroy(GetComponent<CircleCollider2D>());
        StartCoroutine(Bursting());
    }

    /// <summary>
    /// Корутина для анимации лопающегося пузыря.
    /// </summary>
    private IEnumerator Bursting()
    {
        _audioSource.Play();
        foreach (var sprite in spriteBubble)
        {
            _spriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(0.1f);
        }

        WinBobbles.instance.count--;
        Destroy(gameObject);
    }

    /// <summary>
    /// Корутина для самоуничтожения пузыря по таймеру.
    /// </summary>
    private IEnumerator DestroyBubble()
    {
        yield return new WaitForSeconds(10);
        if (_isBursting) yield break;
        _isBursting = true;
        WinBobbles.instance.count--;
        Destroy(gameObject);
    }
}