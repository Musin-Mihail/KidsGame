using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level8MauseClick : MonoBehaviour
{
    public Vector3 Position;
    public static GameObject MoveFigures;
    int layerMask = 1 << 9;

    void OnMouseDown()
    {
        Level8Global.WaitHint = 1;
        // gameObject.GetComponent<MoveItem>().State = 0;
    }
    void OnMouseUp()
    {
        Collider2D hitColliders = Physics2D.OverlapCircle(transform.position, 0.1f, layerMask);
        if(hitColliders != null)
        {
            if(hitColliders.name == gameObject.name)
            {
                // hitColliders.GetComponent<SoundClickItem>().Play();
                transform.position = hitColliders.transform.position;
                Level8Spawn.Count --;
                if(Level8Spawn.Count == 0)
                {
                    Level8Spawn.AllPuzzleStatic[0].GetComponent<Level8MoveAnimal>().end = 1;
                }
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
        if(Input.GetMouseButton(0))
        {
            var _newVector2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _newVector2.z = 0;
            transform.position = _newVector2;
        }
        else if(Input.touchCount > 0)
        {
            var _newVector2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _newVector2.z = 0;
            transform.position = _newVector2;
        }
    }
}