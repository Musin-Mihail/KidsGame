using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Level9MouseClick : MonoBehaviour
{
    Vector3 Position;
    int layerMask = 1 << 9;
    void OnMouseDown()
    {
        Position = GetComponent<MoveItem>().StartPosition;
        Level9Global.WaitHint = 1;
        gameObject.GetComponent<MoveItem>().State = 0;
    }
    void OnMouseUp()
    {
        Collider2D hitColliders = Physics2D.OverlapCircle(transform.position, 0.1f, layerMask);
        if(hitColliders != null)
        {
            if(hitColliders.tag == gameObject.tag)
            {
                hitColliders.GetComponent<SoundClickItem>().Play();
                Destroy(gameObject);
                Level9Spawn.Next = 1;
                WinBobbles.Victory --;
            }
            else
            {
                transform.position = Position;
            }
        }
        else
        {
            transform.position = Position;
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