using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level10Mouse : MonoBehaviour
{
    Vector3 Position;
    int layerMask = 1 << 9;
    void OnMouseDown()
    {
        Position = transform.position;
        Level10.WaitHint = 1;
        gameObject.GetComponent<MoveItem>().State = 0;
    }
    void OnMouseUp()
    {
        Collider2D hitColliders = Physics2D.OverlapCircle(transform.position, 0.1f, layerMask);
        if(hitColliders != null)
        {
            if(hitColliders.transform.localScale.x == gameObject.transform.localScale.x)
            {
                hitColliders.GetComponent<SoundClickItem>().Play();
                Transform[] allChildren = hitColliders.GetComponentsInChildren<Transform>();
                foreach (var item in allChildren)
                {
                    if(item.name == gameObject.name)
                    {
                        item.GetComponent<SpriteRenderer>().enabled = true;
                        Level10.AllBusyPlace.Add(item.gameObject);
                        break;
                    }
                }
                Level10.next --;
                Destroy(gameObject);
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