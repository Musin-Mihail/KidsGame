using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level8MauseClick : MonoBehaviour
{
    public Vector3 Position;
    int layerMask = 1 << 9;
    void OnMouseDown()
    {
        transform.parent.gameObject.GetComponent<Level8>().WaitHint = 1;
        GetComponent<SpriteRenderer>().sortingOrder = 3;
    }
    void OnMouseUp()
    {
        Collider2D hitColliders = Physics2D.OverlapCircle(transform.position, 0.1f, layerMask);
        if(hitColliders != null)
        {
            if(hitColliders.name == gameObject.name)
            {
                hitColliders.GetComponent<SoundClickItem>().Play();
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<SpriteRenderer>().sortingOrder = 1;
                transform.position = hitColliders.transform.position;
                transform.parent.gameObject.GetComponent<Level8>().CountItem --;
                if(transform.parent.gameObject.GetComponent<Level8>().CountItem == 0)
                {
                    transform.parent.gameObject.GetComponent<Level8>().end = 1;
                }
            }
            else
            {
                transform.position = Position;
                GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
        }
        else
        {
            transform.position = Position;
            GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
    }
    void OnMouseDrag()
    {
        // if(Input.GetMouseButton(0))
        // {
        //     var _newVector2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     _newVector2.z = 0;
        //     transform.position = _newVector2;
        // }
        if(Input.touchCount > 0)
        {
            var _newVector2 = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            _newVector2.z = 0;
            transform.position = _newVector2;
        }
    }
}