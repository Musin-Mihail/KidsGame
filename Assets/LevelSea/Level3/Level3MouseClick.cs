using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3MouseClick : MonoBehaviour
{
    public Vector3 Position;
    int layerMask = 1 << 9;

    void OnMouseDown()
    {
        Position = GetComponent<MoveItem>().StartPosition;
        Level3Global.WaitHint = 1;
        gameObject.GetComponent<MoveItem>().State = 0;
    }
    void OnMouseUp()
    {
        Collider2D hitColliders = Physics2D.OverlapCircle(transform.position, 0.1f, layerMask);
        if(hitColliders != null)
        {
            if(hitColliders.name == gameObject.name)
            {
                hitColliders.GetComponent<SoundClickItem>().Play();
                Destroy(gameObject);
                Level3Global.NextFigure = 1;
                Level3Global.ThreeFiguresComplete ++;
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