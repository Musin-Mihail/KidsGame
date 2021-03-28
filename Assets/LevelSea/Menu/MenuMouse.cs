using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMouse : MonoBehaviour
{
    Vector3 Position;
    public GameObject _protection;

    int layerMask = 1 << 9;
    void OnMouseDown()
    {
        Position = transform.position;
    }
    void OnMouseUp()
    {
        Collider2D hitColliders = Physics2D.OverlapCircle(transform.position, 0.1f, layerMask);
        if(hitColliders != null)
        {
            if( hitColliders.name ==  gameObject.name)
            {
                transform.position = Position;
                GetComponent<Protection>().Exit();
                _protection.GetComponent<Protection>().Payment();
                // открываем оплату;
                Debug.Log("Оплата");
            }
            else
            {
                transform.position = Position;
                GetComponent<Protection>().Exit();
                Debug.Log("Не угадал");
            }
        }
        else
        {
            transform.position = Position;
            GetComponent<Protection>().Exit();
            Debug.Log("Мимо");
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